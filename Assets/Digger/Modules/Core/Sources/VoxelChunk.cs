using System;
using System.IO;
using System.Linq;
using System.Text;
using Digger.Modules.Core.Sources.Jobs;
using Digger.Modules.Core.Sources.NativeCollections;
using Digger.Modules.Core.Sources.TerrainInterface;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace Digger.Modules.Core.Sources
{
    public class VoxelChunk : MonoBehaviour
    {
        [SerializeField] private DiggerSystem digger;
        [SerializeField] private int sizeVox;
        [SerializeField] private int sizeOfMesh;
        [SerializeField] private Vector3i chunkPosition;
        [SerializeField] private Vector3i voxelPosition;
        [SerializeField] private Vector3 worldPosition;
        [SerializeField] private ChunkTriggerBounds bounds;

        [NonSerialized] private Voxel[] voxelArray;
        [NonSerialized] private float[] heightArray;
        [NonSerialized] private float3[] normalArray;
        [NonSerialized] private float[] alphamapArray;
        [NonSerialized] private int3 alphamapArraySize;
        [NonSerialized] private int2 alphamapArrayOrigin;

        [NonSerialized] private Voxel[] voxelArrayBeforeOperation;

        [NonSerialized] private JobHandle? currentJobHandle;
        [NonSerialized] private IJobParallelFor currentJob;
        [NonSerialized] private NativeArray<Voxel> voxels;
        [NonSerialized] private NativeArray<float> heights;
        [NonSerialized] private NativeArray<int> holes;
        [NonSerialized] private NativeArray<float3> normals;
        [NonSerialized] private NativeArray<float> alphamaps;
        [NonSerialized] private NativeCounter vertexCounter;
        [NonSerialized] private NativeCounter triangleCounter;
        [NonSerialized] private VoxelKernelModificationJob voxelKernelModificationJob;
        [NonSerialized] private NativeArray<Voxel> voxelsOut;
        [NonSerialized] private MarchingCubesJob.Out mcOut;

        private const MeshUpdateFlags meshUpdateFlags = MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontResetBoneBounds;

        public ChunkTriggerBounds TriggerBounds => bounds;

        public bool IsLoaded => VoxelArray != null && VoxelArray.Length > 0;

        public Vector3i ChunkPosition => chunkPosition;
        public Vector3i VoxelPosition => voxelPosition;

        public float Altitude => voxelPosition.y * digger.HeightmapScale.y;
        public float3 WorldPosition => worldPosition;
        public float3 AbsoluteWorldPosition => digger.transform.TransformPoint(worldPosition);

        public int SizeVox => sizeVox;

        public int SizeOfMesh => sizeOfMesh;

        public float3 HeightmapScale => digger.HeightmapScale;

        public Voxel[] VoxelArray => voxelArray;

        public float[] HeightArray => heightArray;
        public float3[] NormalArray => normalArray;

        public int[] HolesArray => digger.Cutter.GetHoles(chunkPosition, voxelPosition);

        public float3 CutMargin => digger.CutMargin;
        public TerrainCutter Cutter => digger.Cutter;
        public DiggerSystem Digger => digger;

        internal static VoxelChunk Create(DiggerSystem digger, Chunk chunk)
        {
            Utils.Profiler.BeginSample("VoxelChunk.Create");
            var go = new GameObject("VoxelChunk")
            {
                hideFlags = HideFlags.DontSaveInBuild
            };
            go.transform.parent = chunk.transform;
            go.transform.position = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            var voxelChunk = go.AddComponent<VoxelChunk>();
            voxelChunk.digger = digger;
            voxelChunk.sizeVox = digger.SizeVox;
            voxelChunk.sizeOfMesh = digger.SizeOfMesh;
            voxelChunk.chunkPosition = chunk.ChunkPosition;
            voxelChunk.voxelPosition = chunk.VoxelPosition;
            voxelChunk.worldPosition = chunk.WorldPosition;
            voxelChunk.Load();

            Utils.Profiler.EndSample();
            return voxelChunk;
        }

        private static void GenerateVoxels(DiggerSystem digger, float[] heightArray, float chunkAltitude,
            ref Voxel[] voxelArray)
        {
            Utils.Profiler.BeginSample("[Dig] VoxelChunk.GenerateVoxels");
            var sizeVox = digger.SizeVox;
            if (voxelArray == null)
                voxelArray = new Voxel[sizeVox * sizeVox * sizeVox];

            var heights = new NativeArray<float>(heightArray, Allocator.TempJob);
            var voxels = new NativeArray<Voxel>(sizeVox * sizeVox * sizeVox, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);

            // Set up the job data
            var jobData = new VoxelGenerationJob
            {
                ChunkAltitude = chunkAltitude,
                Heights = heights,
                Voxels = voxels,
                SizeVox = sizeVox,
                SizeVox2 = sizeVox * sizeVox,
                HeightmapScale = digger.HeightmapScale,
            };

            // Schedule the job
            var handle = jobData.Schedule(voxels.Length, 64);

            // Wait for the job to complete
            handle.Complete();

            voxels.CopyTo(voxelArray);
            heights.Dispose();
            voxels.Dispose();

            Utils.Profiler.EndSample();
        }

        public void RefreshVoxels()
        {
            Utils.Profiler.BeginSample("VoxelChunk.RefreshVoxels");
            if (VoxelArray == null)
                return;

            heights = new NativeArray<float>(HeightArray, Allocator.TempJob);
            voxels = new NativeArray<Voxel>(VoxelArray, Allocator.TempJob);
            holes = new NativeArray<int>(digger.Cutter.GetHoles(chunkPosition, voxelPosition), Allocator.TempJob);

            // Set up the job data
            var jobData = new VoxelRefreshJob()
            {
                ChunkAltitude = Altitude,
                Heights = heights,
                Holes = holes,
                Voxels = voxels,
                SizeVox = SizeVox,
                SizeVox2 = SizeVox * SizeVox,
                HeightmapScale = digger.HeightmapScale,
            };

            // Schedule the job
            var handle = jobData.Schedule(voxels.Length, 64);

            // Wait for the job to complete
            handle.Complete();

            voxels.CopyTo(VoxelArray);
            heights.Dispose();
            voxels.Dispose();
            holes.Dispose();

            Utils.Profiler.EndSample();
        }

        public void DoOperation<T>(IOperation<T> operation) where T : struct, IJobParallelFor
        {
            var job = operation.Do(this);
            currentJob = job;
            currentJobHandle = job.Schedule(VoxelArray.Length, 64);
        }

        public void CompleteOperation<T>(IOperation<T> operation) where T : struct, IJobParallelFor
        {
            currentJobHandle?.Complete();
            currentJobHandle = null;
            operation.Complete((T)currentJob, this);
#if UNITY_EDITOR
            RecordUndoIfNeeded();
#endif
            digger.EnsureChunkWillBePersisted(this);
        }


        private void DoKernelOperation(ActionType action, float intensity, Vector3 position, float radius)
        {
            voxelArrayBeforeOperation = new Voxel[VoxelArray.Length];
            Array.Copy(VoxelArray, voxelArrayBeforeOperation, VoxelArray.Length);
            voxels = new NativeArray<Voxel>(VoxelArray, Allocator.TempJob);
            voxelsOut = new NativeArray<Voxel>(VoxelArray, Allocator.TempJob);

            heights = new NativeArray<float>(HeightArray, Allocator.TempJob);
            holes = new NativeArray<int>(digger.Cutter.GetHoles(chunkPosition, voxelPosition), Allocator.TempJob);

            voxelKernelModificationJob = new VoxelKernelModificationJob
            {
                SizeVox = SizeVox,
                SizeVox2 = SizeVox * SizeVox,
                SizeOfMesh = SizeOfMesh,
                LowInd = SizeVox - 3,
                Action = action,
                HeightmapScale = digger.HeightmapScale,
                Voxels = voxels,
                VoxelsOut = voxelsOut,
                Intensity = intensity,
                Center = position,
                Radius = radius,
                ChunkAltitude = Altitude,
                Heights = heights,
                Holes = holes,

                NeighborVoxelsLBB = LoadVoxels(digger, chunkPosition + new Vector3i(-1, -1, -1)),
                NeighborVoxelsLBF = LoadVoxels(digger, chunkPosition + new Vector3i(-1, -1, +1)),
                NeighborVoxelsLB_ = LoadVoxels(digger, chunkPosition + new Vector3i(-1, -1, +0)),
                NeighborVoxels_BB = LoadVoxels(digger, chunkPosition + new Vector3i(+0, -1, -1)),
                NeighborVoxels_BF = LoadVoxels(digger, chunkPosition + new Vector3i(+0, -1, +1)),
                NeighborVoxels_B_ = LoadVoxels(digger, chunkPosition + new Vector3i(+0, -1, +0)),
                NeighborVoxelsRBB = LoadVoxels(digger, chunkPosition + new Vector3i(+1, -1, -1)),
                NeighborVoxelsRBF = LoadVoxels(digger, chunkPosition + new Vector3i(+1, -1, +1)),
                NeighborVoxelsRB_ = LoadVoxels(digger, chunkPosition + new Vector3i(+1, -1, +0)),
                NeighborVoxelsL_B = LoadVoxels(digger, chunkPosition + new Vector3i(-1, +0, -1)),
                NeighborVoxelsL_F = LoadVoxels(digger, chunkPosition + new Vector3i(-1, +0, +1)),
                NeighborVoxelsL__ = LoadVoxels(digger, chunkPosition + new Vector3i(-1, +0, +0)),
                NeighborVoxels__B = LoadVoxels(digger, chunkPosition + new Vector3i(+0, +0, -1)),

                NeighborVoxels__F = LoadVoxels(digger, chunkPosition + new Vector3i(+0, +0, +1)),
                NeighborVoxelsR_B = LoadVoxels(digger, chunkPosition + new Vector3i(+1, +0, -1)),
                NeighborVoxelsR_F = LoadVoxels(digger, chunkPosition + new Vector3i(+1, +0, +1)),
                NeighborVoxelsR__ = LoadVoxels(digger, chunkPosition + new Vector3i(+1, +0, +0)),
                NeighborVoxelsLUB = LoadVoxels(digger, chunkPosition + new Vector3i(-1, +1, -1)),
                NeighborVoxelsLUF = LoadVoxels(digger, chunkPosition + new Vector3i(-1, +1, +1)),
                NeighborVoxelsLU_ = LoadVoxels(digger, chunkPosition + new Vector3i(-1, +1, +0)),
                NeighborVoxels_UB = LoadVoxels(digger, chunkPosition + new Vector3i(+0, +1, -1)),
                NeighborVoxels_UF = LoadVoxels(digger, chunkPosition + new Vector3i(+0, +1, +1)),
                NeighborVoxels_U_ = LoadVoxels(digger, chunkPosition + new Vector3i(+0, +1, +0)),
                NeighborVoxelsRUB = LoadVoxels(digger, chunkPosition + new Vector3i(+1, +1, -1)),
                NeighborVoxelsRUF = LoadVoxels(digger, chunkPosition + new Vector3i(+1, +1, +1)),
                NeighborVoxelsRU_ = LoadVoxels(digger, chunkPosition + new Vector3i(+1, +1, +0)),
            };

            // Schedule the job
            currentJobHandle = voxelKernelModificationJob.Schedule(voxels.Length, 64);
        }

        public void UpdateVoxelsOnSurface()
        {
            if (VoxelArray == null)
                return;

            heights = new NativeArray<float>(HeightArray, Allocator.TempJob);
            voxels = new NativeArray<Voxel>(VoxelArray, Allocator.TempJob);
            var cutter = digger.Cutter;
            holes = new NativeArray<int>(cutter.GetHoles(chunkPosition, voxelPosition), Allocator.TempJob);

            // Set up the job data
            var jobData = new VoxelFillSurfaceJob()
            {
                ChunkAltitude = Altitude,
                Heights = heights,
                Voxels = voxels,
                Holes = holes,
                SizeVox = SizeVox,
                SizeVox2 = SizeVox * SizeVox,
                HeightmapScale = digger.HeightmapScale,
            };

            // Schedule the job
            currentJobHandle = jobData.Schedule(voxels.Length, 64);
        }

        public void CompleteUpdateVoxelsOnSurface()
        {
            currentJobHandle?.Complete();
            currentJobHandle = null;
            voxels.CopyTo(VoxelArray);
            heights.Dispose();
            voxels.Dispose();
            holes.Dispose();
        }

        public bool HasAlteredVoxels()
        {
            return VoxelArray != null && VoxelArray.Any(voxel => voxel.Alteration != Voxel.Unaltered);
        }

        public Mesh BuildMesh(int lod)
        {
            var edgeTable = NativeCollectionsPool.Instance.GetMCEdgeTable();
            var triTable = NativeCollectionsPool.Instance.GetMCTriTable();
            var corners = NativeCollectionsPool.Instance.GetMCCorners();
            voxels = new NativeArray<Voxel>(VoxelArray, Allocator.TempJob);
            normals = new NativeArray<float3>(normalArray, Allocator.TempJob);
            alphamaps = new NativeArray<float>(alphamapArray, Allocator.TempJob);
            mcOut = NativeCollectionsPool.Instance.GetMCOut();
            vertexCounter = new NativeCounter(Allocator.TempJob);
            triangleCounter = new NativeCounter(Allocator.TempJob, 3);

            var tData = digger.Terrain.terrainData;
            var alphamapsSize = new int2(tData.alphamapWidth, tData.alphamapHeight);
            var uvScale = new Vector2(1f / tData.size.x, 1f / tData.size.z);
            var scale = new float3(digger.HeightmapScale);// { y = 1f };

            // for retro-compatibility
            if (lod <= 0) lod = 1;

            // Set up the job data
            var jobData = new MarchingCubesJob(edgeTable,
                triTable,
                corners,
                vertexCounter.ToConcurrent(),
                triangleCounter.ToConcurrent(),
                voxels,
                normals,
                alphamaps,

                // out params
                mcOut,

                // misc
                scale,
                uvScale,
                worldPosition,
                lod,
                alphamapArrayOrigin,
                alphamapsSize,
                alphamapArraySize,
                digger.MaterialType);

            jobData.SizeVox = SizeVox;
            jobData.SizeVox2 = SizeVox * SizeVox;
            jobData.Isovalue = 0f;
            jobData.AlteredOnly = 1;
            jobData.FullOutput = 1;
            jobData.IsBuiltInHDRP = (byte)(digger.MaterialType == TerrainMaterialType.HDRP ? 1 : 0);
            

            // Schedule the job
            currentJobHandle = jobData.Schedule(voxels.Length, 4);
            // Wait for the job to complete
            currentJobHandle?.Complete();
            currentJobHandle = null;

            var vertexCount = vertexCounter.Count;
            var triangleCount = triangleCounter.Count;

            voxels.Dispose();
            normals.Dispose();
            alphamaps.Dispose();
            vertexCounter.Dispose();
            triangleCounter.Dispose();

            return ToMesh(mcOut, vertexCount, triangleCount);
        }

        private Mesh ToMesh(MarchingCubesJob.Out o, int vertexCount, int triangleCount)
        {
            if (vertexCount < 3 || triangleCount < 1 || vertexCount >= MarchingCubesJob.MaxVertexCount || triangleCount >= MarchingCubesJob.MaxTriangleCount)
                return null;

            Utils.Profiler.BeginSample("[Dig] VoxelChunk.ToMesh");
            var tData = digger.Terrain.terrainData;

            var mesh = new Mesh();
            AddVertexData(mesh, o, vertexCount, triangleCount);

            mesh.bounds = GetBounds();
            if (digger.MaterialType == TerrainMaterialType.CTS) {
                Utils.Profiler.BeginSample("[Dig] VoxelChunk.ToMesh.RecalculateTangents");
                mesh.RecalculateTangents();
                Utils.Profiler.EndSample();
            }

            Utils.Profiler.EndSample();
            return mesh;
        }
        
        public void BakePhysicMesh(int meshInstanceId)
        {
            var job = new PhysicsBakeMeshJob
            {
                MeshInstanceId = meshInstanceId
            };

            // Schedule the job
            currentJobHandle = job.Schedule();
        }

        public void CompleteBakePhysicMesh()
        {
            currentJobHandle?.Complete();
            currentJobHandle = null;
        }

        private Bounds GetBounds()
        {
            return digger.GetChunkBounds();
        }
        
        private void AddVertexData(Mesh mesh, MarchingCubesJob.Out o, int vertexCount, int triangleCount)
        {
            Utils.Profiler.BeginSample("[Dig] VoxelChunk.AddVertexData");
            mesh.SetVertexBufferParams(vertexCount, VertexData.Layout);
            mesh.SetVertexBufferData(o.outVertexData, 0, 0, vertexCount, 0, meshUpdateFlags);
            mesh.SetIndexBufferParams(triangleCount, IndexFormat.UInt16);
            mesh.SetIndexBufferData(o.outTriangles, 0, 0, triangleCount, meshUpdateFlags);
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, triangleCount), meshUpdateFlags);
            Utils.Profiler.EndSample();
        }

        private void RecordUndoIfNeeded()
        {
#if UNITY_EDITOR
            if (VoxelArray == null || VoxelArray.Length == 0) {
                Debug.LogError("Voxel array should not be null when recording undo");
                return;
            }

            Utils.Profiler.BeginSample("[Dig] VoxelChunk.RecordUndoIfNeeded");
            var path = digger.GetEditorOnlyPathVoxelFile(chunkPosition);

            var savePath = digger.GetPathVersionedVoxelFile(chunkPosition, digger.PreviousVersion);
            if (File.Exists(path) && !File.Exists(savePath)) {
                File.Copy(path, savePath);
            }

            var metadataPath = digger.GetEditorOnlyPathVoxelMetadataFile(chunkPosition);

            var saveMetadataPath = digger.GetPathVersionedVoxelMetadataFile(chunkPosition, digger.PreviousVersion);
            if (File.Exists(metadataPath) && !File.Exists(saveMetadataPath)) {
                File.Copy(metadataPath, saveMetadataPath);
            }

            Utils.Profiler.EndSample();
#endif
        }

        public void Persist()
        {
            if (VoxelArray == null || VoxelArray.Length == 0) {
                Debug.LogError("Voxel array should not be null in saving");
                return;
            }

            Utils.Profiler.BeginSample("[Dig] VoxelChunk.Persist");
            var path = digger.GetPathVoxelFile(chunkPosition, true);

            var voxelsToPersist = new NativeArray<Voxel>(VoxelArray, Allocator.Temp);
            var bytes = new NativeSlice<Voxel>(voxelsToPersist).SliceConvert<byte>();
            File.WriteAllBytes(path, bytes.ToArray());
            voxelsToPersist.Dispose();

            var metadataPath = digger.GetPathVoxelMetadataFile(chunkPosition, true);
            using (var stream = new FileStream(metadataPath, FileMode.Create, FileAccess.Write, FileShare.Write, 4096,
                FileOptions.Asynchronous)) {
                using (var writer = new BinaryWriter(stream, Encoding.Default)) {
                    writer.Write(bounds.IsVirgin);
                    writer.Write(bounds.Min.x);
                    writer.Write(bounds.Min.y);
                    writer.Write(bounds.Min.z);
                    writer.Write(bounds.Max.x);
                    writer.Write(bounds.Max.y);
                    writer.Write(bounds.Max.z);
                }
            }

#if UNITY_EDITOR
            var savePath = digger.GetPathVersionedVoxelFile(chunkPosition, digger.Version);
            File.Copy(path, savePath, true);
            var saveMetadataPath = digger.GetPathVersionedVoxelMetadataFile(chunkPosition, digger.Version);
            File.Copy(metadataPath, saveMetadataPath, true);
#endif
            Utils.Profiler.EndSample();
        }

        public void Load()
        {
            Utils.Profiler.BeginSample("VoxelChunk.Load");
            // Feed heights again in case they have been modified
            heightArray = digger.HeightsFeeder.GetHeights(chunkPosition, voxelPosition);
            normalArray = digger.NormalsFeeder.GetNormals(chunkPosition, voxelPosition);
            var alphamapsInfo = digger.AlphamapsFeeder.GetAlphamaps(chunkPosition, worldPosition, SizeOfMesh);
            alphamapArray = alphamapsInfo.AlphamapArray;
            alphamapArraySize = alphamapsInfo.AlphamapArraySize;
            alphamapArrayOrigin = alphamapsInfo.AlphamapArrayOrigin;

            var path = digger.GetPathVoxelFile(chunkPosition, false);
            var rawBytes = Utils.GetBytes(path);

            if (rawBytes == null) {
                if (VoxelArray == null) {
                    // If there is no persisted voxels but voxel array is null, then we fallback and (re)generate them.
                    GenerateVoxels(digger, HeightArray, Altitude, ref voxelArray);
                    digger.EnsureChunkWillBePersisted(this);
                }

                Utils.Profiler.EndSample();
                return;
            }

            ReadVoxelFile(SizeVox, rawBytes, ref voxelArray);

            var hScale = digger.HeightmapScale;
            var metadataPath = digger.GetPathVoxelMetadataFile(chunkPosition, false);
            rawBytes = Utils.GetBytes(metadataPath);
            if (rawBytes == null) {
                Debug.LogError($"Could not read metadata file of chunk {chunkPosition}");
                Utils.Profiler.EndSample();
                return;
            }

            using (Stream stream = new MemoryStream(rawBytes)) {
                using (var reader = new BinaryReader(stream, Encoding.Default)) {
                    bounds = new ChunkTriggerBounds(
                        reader.ReadBoolean(),
                        new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
                        new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
                        hScale, SizeOfMesh);
                }
            }

            Utils.Profiler.EndSample();
        }

        public void InitVoxelArrayBeforeOperation()
        {
            voxelArrayBeforeOperation = new Voxel[VoxelArray.Length];
            Array.Copy(VoxelArray, voxelArrayBeforeOperation, VoxelArray.Length);
        }

        internal void ResetVoxelArrayBeforeOperation()
        {
            voxelArrayBeforeOperation = null;
        }

        private static void ReadVoxelFile(int sizeVox, byte[] rawBytes, ref Voxel[] voxelArray)
        {
            if (voxelArray == null)
                voxelArray = new Voxel[sizeVox * sizeVox * sizeVox];

            var voxelBytes = new NativeArray<byte>(rawBytes, Allocator.Temp);
            var bytes = new NativeSlice<byte>(voxelBytes);
            var voxelSlice = bytes.SliceConvert<Voxel>();
            DirectNativeCollectionsAccess.CopyTo(voxelSlice, voxelArray);
            voxelBytes.Dispose();
        }

        public static NativeArray<Voxel> LoadVoxels(DiggerSystem digger, Vector3i chunkPosition)
        {
            Utils.Profiler.BeginSample("[Dig] VoxelChunk.LoadVoxels");

            if (!digger.IsChunkBelongingToMe(chunkPosition)) {
                var neighbor = digger.GetNeighborAt(chunkPosition);
                if (neighbor) {
                    var neighborChunkPosition = neighbor.ToChunkPosition(digger.ToWorldPosition(chunkPosition));
                    if (!neighbor.IsChunkBelongingToMe(neighborChunkPosition)) {
                        Debug.LogError(
                            $"neighborChunkPosition {neighborChunkPosition} should always belong to neighbor");
                        return new NativeArray<Voxel>(1, Allocator.TempJob);
                    }

                    return LoadVoxels(neighbor, neighborChunkPosition);
                }
            }

            if (digger.GetChunk(chunkPosition, out var chunk)) {
                if (chunk.VoxelChunk.voxelArrayBeforeOperation != null) {
                    return new NativeArray<Voxel>(chunk.VoxelChunk.voxelArrayBeforeOperation, Allocator.TempJob);
                }

                chunk.LazyLoad();
                return new NativeArray<Voxel>(chunk.VoxelChunk.VoxelArray, Allocator.TempJob);
            }

            return new NativeArray<Voxel>(1, Allocator.TempJob);
        }
    }
}
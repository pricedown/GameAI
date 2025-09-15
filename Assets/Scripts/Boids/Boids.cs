using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boids
{
    struct Boid
    {
        public Vector3 position;
        public Vector3 velocity;

        public Boid(Vector3 pos, Vector3 vel)
        {
            position = pos;
            velocity = vel;
        }
    }

    [System.Serializable]
    public struct BoidParams
    {
        [Header("Movement")]
        public float moveSpeed;
        public float turnSpeed;
        
        [Header("Alignment")] 
        public bool isAlignmentEnabled;
        public float alignmentRadius;
        public float alignmentWeight;
            
        [Header("Cohesion")] 
        public bool isCohesionEnabled;
        public float cohesionRadius;
        public float cohesionWeight;
        
        [Header("Separation")] 
        public bool isSeparationEnabled;
        public float separationRadius;
        public float separationWeight;
    }
    
    public class Boids : MonoBehaviour
    {
        private static readonly int DeltaTime = Shader.PropertyToID("deltaTime");
        
        public ComputeShader computeShader;

        [Header("Bounds")] 
        public float boundsRadius = 100;
        public float boundsTurnSpeed = 3;
        
        [Header("Spawning")]
        public int spawnedBoids;
        public float spawnRadius;
        
        [Header("Boid Behavior")]
        public BoidParams boidParams;
        
        [Header("Boid Visuals")]
        public Mesh boidMesh;
        public Material boidMaterial;
        private GraphicsBuffer _argsBuffer;
        private RenderParams _renderParams;

        private Boid[] _boids;
        private int _kernel;
        private ComputeBuffer _boidsBuffer;
        private int _groupSizeX;
        private int _numOfBoids;
        
        void Start()
        {
            _kernel =  computeShader.FindKernel("CSMain");
            uint x;
            computeShader.GetKernelThreadGroupSizes(_kernel, out x, out _, out _);
            _groupSizeX = Mathf.CeilToInt(spawnedBoids / (float)x);
            _numOfBoids = _groupSizeX * (int)x;
            
            InitBoids();
            InitShader();
        }

        private void InitBoids()
        {
            _boids = new Boid[_numOfBoids];

            for (int i = 0; i < _numOfBoids; i++)
            {
                Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
                Vector3 vel = Random.insideUnitSphere * boidParams.moveSpeed;
                _boids[i] = new Boid(pos, vel);
            }
        }

        void InitShader()
        {
            
            int stride = sizeof(float) * 6;
            _boidsBuffer = new ComputeBuffer(_numOfBoids, stride);
            _boidsBuffer.SetData(_boids);
            
            _argsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, GraphicsBuffer.IndirectDrawIndexedArgs.size);
            GraphicsBuffer.IndirectDrawIndexedArgs[] data = new GraphicsBuffer.IndirectDrawIndexedArgs[1];
            data[0].indexCountPerInstance = boidMesh.GetIndexCount(0);
            data[0].instanceCount = (uint)_numOfBoids;
            _argsBuffer.SetData(data);
            
            computeShader.SetBuffer(_kernel, "boidsBuffer", _boidsBuffer);
            UpdateParams(boidParams);
            boidMaterial.SetBuffer("boidsBuffer", _boidsBuffer);
            computeShader.SetFloat("boundsRadius", boundsRadius);
            computeShader.SetFloat("boundsTurnSpeed", boundsTurnSpeed);
            
            _renderParams = new RenderParams(boidMaterial);
            _renderParams.worldBounds = new Bounds(Vector3.zero, Vector3.one * 100000);
        }

        public void UpdateParams(BoidParams boidParameters)
        {
            computeShader.SetInt("boidsCount", _numOfBoids);
            computeShader.SetFloat("moveSpeed", boidParameters.moveSpeed);
            computeShader.SetFloat("turnSpeed", boidParameters.turnSpeed);

            computeShader.SetBool("isAlignmentEnabled", boidParameters.isAlignmentEnabled);
            computeShader.SetFloat("alignmentRadius", boidParameters.alignmentRadius);
            computeShader.SetFloat("alignmentWeight", boidParameters.alignmentWeight);

            computeShader.SetBool("isCohesionEnabled", boidParameters.isCohesionEnabled);
            computeShader.SetFloat("cohesionRadius", boidParameters.cohesionRadius);
            computeShader.SetFloat("cohesionWeight", boidParameters.cohesionWeight);

            computeShader.SetBool("isSeparationEnabled", boidParameters.isSeparationEnabled);
            computeShader.SetFloat("separationRadius", boidParameters.separationRadius);
            computeShader.SetFloat("separationWeight", boidParameters.separationWeight);
            
            Debug.Log("BoidParams updated");
        }

        void Update()
        {
            computeShader.SetFloat(DeltaTime, Time.deltaTime);
            computeShader.Dispatch(_kernel, _groupSizeX, 1, 1); 
            Graphics.RenderMeshIndirect(_renderParams, boidMesh, _argsBuffer);
        }

        void OnDestroy()
        {
            if (_boidsBuffer != null)
                _boidsBuffer.Dispose();
            
            if (_argsBuffer != null)
                _argsBuffer.Dispose();
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (_boidsBuffer == null || !Application.isPlaying) 
                return;
            Gizmos.color = Color.green;

            Boid[] debugBoids = new Boid[_numOfBoids];
            _boidsBuffer.GetData(debugBoids);

            foreach (Boid boid in debugBoids)
            {
                Gizmos.DrawSphere(boid.position, 0.25f);
                Gizmos.DrawLine(boid.position, boid.position + boid.velocity.normalized * 2.2f);
            }
        }
#endif
    }
}

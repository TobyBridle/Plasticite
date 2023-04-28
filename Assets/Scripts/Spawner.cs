using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // We want a reference to the spawner object
    public GameObject spawner;

    // We want a list of the prefabs which we can spawn
    public List<GameObject> prefabs;
    private List<float> _prefabWeights;

    // We want a base rate which is the lowest rate at which we can spawn prefabs per second.
    public float baseSpawnRate = 1f;

    private int _difficulty;
    private int _wave;
    private float _spawnRate; // We will alter this value based on the difficulty and wave
    private int _maxEnemies, _spawnCount = 0; // This value will also depend on the difficulty and wave

    private float _tick = 0f; // This will be used to keep track of how much time has passed since the last spawn

    // We want to have an animation on instantiation
    // and an animation that is constant
    // To do this, we need to have a reference to Animator
    private Animator _animator;
    public AnimationClip spawnAnimation;
    public AnimationClip idleAnimation;
    
    // We want a hint to the player about what key they should press to progress
    public GameObject hint;

    // Start is called before the first frame update
    void Start()
    {
        // We want to get the difficulty from the user prefs
        // The difficulty will determine the ratio of prefabs we want to spawn.
        // A higher difficulty will spawn more prefabs from the end of our list and spawn ALL prefabs at a higher rate
        // A lower difficulty will spawn more prefabs from the start of our list and spawn ALL prefabs at a lower rate.
        // The difficulty will be a number between 1 and 3 (inclusive)
        _difficulty = PlayerPrefs.GetInt("difficulty", 1); // 1 is the default value

        // The wave is equivalent to the level that the player is on.
        _wave = PlayerPrefs.GetInt("wave", 1); // 1 is the default value

        // We want to alter the spawn rate depending on the wave.
        // This value should get closer to 0.8s as the wave increases.
        // We should use an exponential function with a base of 0.8
        // On the first wave, we want the spawn rate to be close to 2s
        _spawnRate = baseSpawnRate * Mathf.Pow(0.8f, _wave) + 0.2f;
        

        _maxEnemies = 4 + _wave;
        switch (_difficulty) {
            case 2:
                _maxEnemies = Mathf.FloorToInt(_maxEnemies * 1.5f);
                break;
            case 3:
                _maxEnemies = Mathf.FloorToInt(_maxEnemies * 2f);
                break;
        }

        // We want to generate a list of weights for our discrete distribution
        // For example, on Wave 1, Difficulty 1, with 3 prefabs, our DRV will be:
        // 0.9, 0.075, 0.025
        _prefabWeights = new List<float>();
        // We will make a closure which mimics the behaviour of a normal distribution
        // We will use this to generate our weights
        float normalDistribution(float x, float mean, float stdDev) {
            return Mathf.Exp(-Mathf.Pow(x - mean, 2) / (2 * Mathf.Pow(stdDev, 2))) / (stdDev * Mathf.Sqrt(2 * Mathf.PI));
        }

        // We want to generate a list of weights for our discrete distribution
        for (int i = 0; i < prefabs.Count; i++) {
            // We want to adjust the mean and standard deviation based on the difficulty & wave
            // The lower the wave, the lower the mean
            float mean = (prefabs.Count - 1) * (1f - (1f / _wave));
            float stdDev = (prefabs.Count - 1) / (4f * _difficulty) * Mathf.Pow(1.5f, _wave);
            float weight = normalDistribution(i, mean, stdDev);
            _prefabWeights.Add(weight);
        }

        // We want to get the animator component
        _animator = GetComponent<Animator>();
        _animator.Play(spawnAnimation.name);
        // We want to play the idle animation after the spawn animation has finished
        StartCoroutine(PlayIdleAnimation());
    }

    // PlayIdleAnimation is a coroutine which will play the idle animation after the spawn animation has finished
    IEnumerator PlayIdleAnimation() {
        yield return new WaitForSeconds(spawnAnimation.length);
        _animator.Play(idleAnimation.name);
    }

    // Update is called once per frame
    void Update()
    {
        // Select the prefab we want to spawn
        // Generate a randomn number biased using the difficulty
        if (_spawnCount < _maxEnemies && _tick > _spawnRate) {
            _tick = 0f;
            
            // Generate a random number between 0 and 1
            // Find the weight which is closest to this number
            float randomNumber = Random.Range(0f, 1f);
            int closest = 0;
            float closestDistance = Mathf.Abs(randomNumber - _prefabWeights[0]);
            for (int i = 1; i < _prefabWeights.Count; i++) {
                float distance = Mathf.Abs(randomNumber - _prefabWeights[i]);
                if (distance < closestDistance) {
                    closest = i;
                    closestDistance = distance;
                }
            }

            // Get the prefab
            GameObject prefab = prefabs[closest];

            // Spawn the prefab
            Instantiate(prefab, spawner.transform.position, Quaternion.identity);
            _spawnCount += 1;
        }
        _tick += Time.deltaTime;
   }

    public void showText() {
        hint.SetActive(true);
    }

    public void hideText() {
        hint.SetActive(false);
    }
}

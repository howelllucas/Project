using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MyRandom
    {
        public struct State
        {
            public int seed;
            public int counter;
            public State(int seed, int counter)
            {
                this.seed = seed;
                this.counter = counter;
            }
        }

        private System.Random random;
        private int seed;
        private int counter;

        public float Value
        {
            get
            {
                counter++;
                return (float)random.NextDouble();
            }
        }
        public State state
        {
            get { return new State(seed, counter); }
            set
            {
                seed = value.seed;
                counter = value.counter;
                ResetState();
            }
        }

        public MyRandom()
        {
            seed = Random.Range(0, int.MaxValue);
            random = new System.Random(seed);
        }

        public MyRandom(State state)
        {
            this.seed = state.seed;
            this.counter = state.counter;
            ResetState();
        }

        public void ResetState()
        {
            random = new System.Random(seed);
            for (int i = 0; i < counter; i++)
            {
                random.Next();
            }
        }

        public int Range(int min, int max)
        {
            counter++;
            return random.Next(min, max);
        }

        public float Range(float min, float max)
        {
            counter++;
            return min + (float)(random.NextDouble() * (max - min));
        }
    }
}
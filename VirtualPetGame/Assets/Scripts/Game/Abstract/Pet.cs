using System;

public class Pet
{
    public string lastTimeFed, lastTimeHappy, lastTimeGainedEnergy;
    public int food, happiness, energy, age;
    public float timePassed;

    public Pet(string lastTimeFed, string lastTimeHappy, string lastTimeGainedEnergy, int food, int happiness, int energy, int age, float timePassed)
    {
        this.lastTimeFed = lastTimeFed;
        this.lastTimeHappy = lastTimeHappy;
        this.lastTimeGainedEnergy = lastTimeGainedEnergy;
        this.food = food;
        this.happiness = happiness;
        this.energy = energy;
        this.age = age;
        this.timePassed = timePassed;

    }
}

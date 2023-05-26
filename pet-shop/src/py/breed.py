import random
import sys
import json
import os 

# Define the genetic traits for the virtual pets
traits = {
    'age': [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
    'body_colour': ['gray', 'red', 'orange', 'yellow', 'green', 'blue', 'indigo', 'pink'],
    'maxfood_level': ['very_low', 'low', 'medium', 'high', 'very_high'],
    'maxhappiness_level': ['very_low', 'low', 'medium', 'high', 'very_high'],
    'maxenergy_level': ['very_low', 'low', 'medium', 'high', 'very_high'],
    'hunger_level': ['very_low', 'low', 'medium', 'high', 'very_high'],
    'sadness_level': ['very_low', 'low', 'medium', 'high', 'very_high'],
    'laziness_level': ['very_low', 'low', 'medium', 'high', 'very_high'],
    'fertility_counter': [1, 2, 3, 4, 5, 6, 7]
}

# Define the weights for each trait's fitness function
weights = {
    'age': 0.1,
    'maxfood_level': 0.1,
    'maxhappiness_level': 0.1,
    'maxenergy_level': 0.1,
    'hunger_level': 0.125,
    'sadness_level': 0.125,
    'laziness_level': 0.125,
    'fertility_counter': 0.3
}

class VirtualPet:
    def __init__(self, id, name, age, body_colour, maxfood_level, maxhappiness_level,
                 maxenergy_level, hunger_level, sadness_level, laziness_level, fertility_counter,
                 picture):
        self.id = id
        self.name = name
        self.age = age
        self.body_colour = body_colour
        self.maxfood_level = maxfood_level
        self.maxhappiness_level = maxhappiness_level
        self.maxenergy_level = maxenergy_level
        self.hunger_level = hunger_level
        self.sadness_level = sadness_level
        self.laziness_level = laziness_level
        self.fertility_counter = fertility_counter
        self.picture = picture

    def __str__(self):
        return f"VirtualPet(id={self.id}, name={self.name}, size={self.age},  body_colour={self.body_colour},  maxfood_level={self.maxfood_level}, maxhappiness_level={self.maxhappiness_level}, maxenergy_level={self.maxenergy_level},  hunger_level={self.hunger_level},  sadness_level={self.sadness_level},  laziness_level={self.laziness_level},  fertility_counter{self.fertility_counter})"

# Initializing the population


def create_population(json_file):

    with open(json_file) as f:
        data = json.load(f)

    population = []
    for pet_data in data:
        pet = VirtualPet(**pet_data)
        population.append(pet)

    return population

# ______________________________________

# Evaluating the fitness of each pet

# Max Food Level
# Max Happiness Level
# Max Energy Level
def traits_fitness(value):
    if value == 'very_low':
        return 1
    elif value == 'low':
        return 2
    elif value == 'medium':
        return 3
    elif value == 'high':
        return 4
    elif value == 'very_high':
        return 5
    
# Hunger Level
# Sadness Level
# Laziness Level
def needs_fitness(value):
    if value == 'very_low':
        return 5
    elif value == 'low':
        return 4
    elif value == 'medium':
        return 3
    elif value == 'high':
        return 2
    elif value == 'very_high':
        return 1

def calculate_fitness(pet):
    # Calculate the score for each trait's fitness function
    age_score = pet.age
    maxfood_score = traits_fitness(pet.maxfood_level)
    maxhappiness_score = traits_fitness(pet.maxhappiness_level)
    maxenergy_score = traits_fitness(pet.maxenergy_level)
    hunger_score = needs_fitness(pet.hunger_level)
    sadness_score = needs_fitness(pet.sadness_level)
    laziness_score = needs_fitness(pet.laziness_level)
    fertility_score = pet.fertility_counter

    # Calculate the weighted sum of the trait scores
    final_score = (age_score * weights['age'] +
                   maxfood_score * weights['maxfood_level'] +
                   maxhappiness_score * weights['maxhappiness_level'] +
                   maxenergy_score * weights['maxenergy_level'] +
                   hunger_score * weights['hunger_level'] +
                   sadness_score * weights['sadness_level'] +
                   laziness_score * weights['laziness_level'] +
                   fertility_score * weights['fertility_counter'])

    return final_score

# fitness score of pet  98 :  1.0 WORST
# fitness score of pet  99 :  5.6 BEST


def get_rating(score):
    if score == 1:
        return 'Very Poor'
    elif score < 2:
        return 'Poor'
    elif score < 3:
        return 'Average'
    elif score < 4:
        return 'Good'
    elif score < 5:
        return 'Great'
    elif score == 5.6:
        return 'Perfect'
    else:
        return 'other'


def select_parents(json_file):
    # Read selected parents from JSON file
    with open(json_file) as f:
        selected_parents = json.load(f)

    # Return two parents for breeding
    return [selected_parents[0], selected_parents[1]]


def crossover(parent1, parent2, mutation_probability):
    child_attributes = {}
    for attr_name in parent1.__dict__:
        # randomly choose which parent to get the attribute from
        if random.random() < 0.5:
            child_attributes[attr_name] = getattr(parent1, attr_name)
        else:
            child_attributes[attr_name] = getattr(parent2, attr_name)

    # create a new VirtualPet instance with the child attributes
    child = VirtualPet(**child_attributes)

    # randomly decide whether to apply mutation or not
    if random.random() < mutation_probability:
        mutate(child)

    return child


def mutate(child):
    # Choose a random attribute to mutate
    attribute = random.choice(['body_colour', 'maxfood_level', 'maxhappiness_level',
                               'maxenergy_level', 'hunger_level', 'sadness_level',
                               'laziness_level'])

    # Modify the attribute
    if attribute == 'body_colour':
        child.body_colour = random.choice(
            ['gray', 'red', 'orange', 'yellow', 'green', 'blue', 'indigo', 'pink'])
    elif attribute == 'maxfood_level':
        child.maxfood_level = random.choice(
            ['very_low', 'low', 'medium', 'high', 'very_high'])
    elif attribute == 'maxhappiness_level':
        child.maxhappiness_level = random.choice(
            ['very_low', 'low', 'medium', 'high', 'very_high'])
    elif attribute == 'maxenergy_level':
        child.maxenergy_level = random.choice(
            ['very_low', 'low', 'medium', 'high', 'very_high'])
    elif attribute == 'hunger_level':
        child.hunger_level = random.choice(
            ['very_low', 'low', 'medium', 'high', 'very_high'])
    elif attribute == 'sadness_level':
        child.sadness_level = random.choice(
            ['very_low', 'low', 'medium', 'high', 'very_high'])
    elif attribute == 'laziness_level':
        child.laziness_level = random.choice(
            ['very_low', 'low', 'medium', 'high', 'very_high'])

    return child


def breed(data):
    # Creating the Initial Population
    json_file = 'json/pets_metadata.json'
    population = create_population(json_file)
    mutation_probability = 0.1

    # selecting the parents
    for pet in population:
        if (int(pet.id) == int(data[0])):
            parent1 = pet
            # print("Parent 1: ", parent1)
        elif (int(pet.id) == int(data[1])):
            parent2 = pet
            # print("Parent 2: ", parent2)

    fitness_parent1 = calculate_fitness(parent1)
    rating_parent1 = get_rating(fitness_parent1)
    fitness_parent2 = calculate_fitness(parent2)
    rating_parent2 = get_rating(fitness_parent2)

    # crossover using parent1 and parent2
    if parent1 is None or parent2 is None:
        error = "Could not find one or both parents in population"
        return error

    else:
        child = crossover(parent1, parent2, mutation_probability)

        print("Child: ", child)
        return child


def main():
    # Creating the Initial Population
    # Get the directory of the current script
    current_dir = os.path.dirname(os.path.abspath('src/'))

    # Path to the JSON file relative to the current script
    json_file = os.path.join(current_dir, 'json/pets_metadata.json')    
    population = create_population(json_file)

    # GET PARENT ID FROM JS
    parent1_id = 9
    parent2_id = 10
    parent1 = None
    parent2 = None

    # selecting the parents
    for pet in population:
        if (int(pet.id) == int(parent1_id)):
            parent1 = pet
            # print("Parent 1: ", parent1)
        elif (int(pet.id) == int(parent2_id)):
            parent2 = pet
            # print("Parent 2: ", parent2)

    # 0.1: 10% chance of mutation
    mutation_probability = 0.1

    # for pet in population:
    #     print("fitness score of pet ", pet.id, ": ", calculate_fitness(pet))

    # selecting the parents
    for pet in population:
        if (int(pet.id) == int(parent1_id[0])):
            parent1 = pet
            # print("Parent 1: ", parent1)
        elif (int(pet.id) == int(parent2_id[1])):
            parent2 = pet
            # print("Parent 2: ", parent2)

    fitness_parent1 = calculate_fitness(parent1)
    rating_parent1 = get_rating(fitness_parent1)
    fitness_parent2 = calculate_fitness(parent2)
    rating_parent2 = get_rating(fitness_parent2)

    # crossover using parent1 and parent2
    if parent1 is None or parent2 is None:
        print("Could not find one or both parents in population")

    else:
        child = crossover(parent1, parent2, mutation_probability)

        print("Child: ", child)

main()
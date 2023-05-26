import random
import json
import os

# List of pet names to choose from
names = ['Buddy', 'Lucky', 'Charlie', 'Rocky', 'Max', 'Jack', 'Toby',
         'Bailey', 'Daisy', 'Molly', 'Lucy', 'Bella', 'Sadie', 'Maggie',
         'Luna', 'Max', 'Charlie', 'Lucy', 'Rocky', 'Bella', 'Daisy',
         'Oliver', 'Milo', 'Bailey', 'Teddy', 'Ruby', 'Pepper', 'Gizmo',
         'Simba', 'Coco', 'Oscar', 'Rosie', 'Jasper', 'Tilly', 'Finn',
         'Poppy', 'Archie', 'Sadie', 'Chester', 'Mimi', 'Zeus', 'Molly',
         'Rusty', 'Gatsby', 'Lulu', 'Toby', 'Penny', 'Rocco', 'Hazel',
         'Zeus', 'Nala', 'Duke', 'Winnie', 'Gus', 'Bella', 'Biscuit',
         'Marley', 'Olive', 'Zeus', 'Daisy', 'Louie', 'Koda', 'Blue']

# Define the genetic traits for the virtual pets
traits = {
    'age': {1: 0.35, 2: 0.35, 3: 0.35, 4: 0.3, 5: 0.3, 6: 0.3, 7: 0.25, 8: 0.25, 9: 0.25, 10: 0.25, 11: 0.25, 12: 0.2, 13: 0.2, 14: 0.2, 15: 0.15, 16: 0.15, 17: 0.15, 18: 0.15, 19: 0.1, 20: 0.1},
    'body_colour': {'gray': 0.1, 'red': 0.1, 'orange': 0.1, 'yellow': 0.1, 'green': 0.1, 'blue': 0.1, 'purple': 0.1, 'pink': 0.1},
    'maxfood_level': {'very_low': 0.4, 'low': 0.3, 'medium': 0.2, 'high': 0.15, 'very_high': 0.1},
    'maxhappiness_level': {'very_low': 0.4, 'low': 0.3, 'medium': 0.2, 'high': 0.15, 'very_high': 0.1},
    'maxenergy_level': {'very_low': 0.4, 'low': 0.3, 'medium': 0.2, 'high': 0.15, 'very_high': 0.1},
    'hunger_level': {'very_low': 0.1, 'low': 0.15, 'medium': 0.2, 'high': 0.3, 'very_high': 0.4},
    'sadness_level': {'very_low': 0.1, 'low': 0.1, 'medium': 0.2, 'high': 0.3, 'very_high': 0.4},
    'laziness_level': {'very_low': 0.1, 'low': 0.15, 'medium': 0.2, 'high': 0.3, 'very_high': 0.4},
    'fertility_counter': {1: 0.3, 2: 0.3, 3: 0.25, 4: 0.2, 5: 0.2, 6: 0.15, 7: 0.1}
}

# Define a dictionary of color-image directory mappings
color_dirs = {
    'gray': 'images/gray_body.png',
    'red': 'images/red_body.png',
    'orange': 'images/orange_body.png',
    'yellow': 'images/yellow_body.png',
    'green': 'images/green_body.png',
    'blue': 'images/blue_body.png',
    'purple': 'images/purple_body.png',
    'pink': 'images/pink_body.png'
}

# Generate a random NFT metadata
metadata_list = []

print("How many pets: ")
pet_count = int(input())

last_name = ''
for i in range(pet_count):
    while True:
        name = random.choice(names)
        if name != last_name:
            break
    last_name = name

    metadata = {'id': str(i), 'name': name}

    for trait, values in traits.items():
        metadata[trait] = random.choices(
            list(values.keys()), list(values.values()))[0]

    # Choose an image directory based on the pet's body colour
    metadata['picture'] = color_dirs[metadata['body_colour']]

    metadata_list.append(metadata)

# Save the metadata list as a JSON file
with open('src/json/pets_metadata.json', 'w') as outfile:
    outfile.write(json.dumps(metadata_list, indent=4))

if os.path.isfile('src/json/pets_metadata.json'):
    print("File successfully created!")
else:
    print("File creation error. Try again!")

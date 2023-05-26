#!/usr/bin/env python
# coding: utf-8

import pandas as pd
import numpy as np
import time
import os
import json
import csv

from copy import deepcopy
from progressbar import progressbar

import warnings
warnings.simplefilter(action='ignore', category=FutureWarning)


# Base metadata. MUST BE EDITED.
BASE_IMAGE_URL = ""
BASE_NAME = ""

BASE_JSON = {
    "name": BASE_NAME,  
    "image": BASE_IMAGE_URL,
    "size": [],
    "body_colour": [],
    "food_level": [],
    "happiness_level": [],
    "energy_level": [],
    "hunger_level": [],
    "sadness_level": [],
    "fitness_level": [],
    "fertility_counter": [],
}


# Get metadata and JSON files path based on edition
def generate_paths(edition_name):
    edition_path = os.path.join('src', 'edition ' + str(edition_name))
    metadata_path = os.path.join(edition_path, 'metadata.csv')

    return edition_path, metadata_path

# Function to convert snake case to sentence case
def clean_attributes(attr_name):
    
    clean_name = attr_name.replace('_', ' ')
    clean_name = list(clean_name)
    
    for idx, ltr in enumerate(clean_name):
        if (idx == 0) or (idx > 0 and clean_name[idx - 1] == ' '):
            clean_name[idx] = clean_name[idx].upper()
    
    clean_name = ''.join(clean_name)
    return clean_name

# Function to get attribure metadata
def get_attribute_metadata(metadata_path):

    # Read attribute data from metadata file 
    df = pd.read_csv(metadata_path)
    df = df.drop('id', axis = 1)
    df.columns = [clean_attributes(col) for col in df.columns]

    # Get zfill count based on number of images generated
    # -1 according to nft.py. Otherwise not working for 100 NFTs, 1000 NTFs, 10000 NFTs and so on
    zfill_count = len(str(df.shape[0]-1))

    return df, zfill_count

def make_json(csvFilePath):
 
    # create a list to store data
    data = []

    # Open a csv reader called DictReader
    with open(csvFilePath) as csvf:
        csvReader = csv.DictReader(csvf)
         
        # Convert each row into a dictionary
        # and add it to data
        for i, rows in enumerate(csvReader):
             
            # Assuming a column named 'No' to
            # be the primary key
            key = rows['']

            rows['image'] = 'edition base/images/' + key.zfill(2) + '.png'
            
            # Replace the empty first field with 'name'
            rows['id'] = rows.pop('')
            rows['name'] = 'Pet ' + str(i+1)
            rows['body'] = rows.pop('body')
            rows["size"] = rows.pop('size')
            rows["body_colour"] = rows.pop('body_colour')
            rows["maxfood_level"] = rows.pop('maxfood_level')
            rows["maxhappiness_level"] = rows.pop('maxhappiness_level')
            rows["maxenergy_level"] = rows.pop('maxenergy_level')
            rows['hunger_level'] = rows.pop('hunger_level')
            rows['sadness_level'] = rows.pop('sadness_level')
            rows['laziness_level'] = rows.pop('laziness_level')
            rows['fertility_counter'] = rows.pop('fertility_counter')

            rows['image'] = rows.pop('image')

            # Append the modified row to data
            data.append(rows)

    # Open a json writer, and use the json.dumps() function to dump data
    with open("src/json/pets.json", 'w') as jsonf:
        jsonf.write(json.dumps(data, indent=4))


# Main function that generates the JSON metadata
def main():

    # Get edition name
    print("Enter edition you want to generate metadata for: ")
    while True:
        edition_name = input()
        edition_path, metadata_path = generate_paths(edition_name)

        if os.path.exists(edition_path):
            print("Edition exists! Generating JSON metadata...")
            break
        else:
            print("Oops! Looks like this edition doesn't exist! Check your output folder to see what editions exist.")
            print("Enter edition you want to generate metadata for: ")
            continue
    
    # Get attribute data and zfill count
    #df, zfill_count = get_attribute_metadata(metadata_path)
    
    make_json(metadata_path)

    # for idx, row in progressbar(df.iterrows()):    
    
    #     # Get a copy of the base JSON (python dict)
    #     item_json = deepcopy(BASE_JSON)
        
    #     # Append number to base name
    #     item_json['id'] = item_json['id'] + str(idx)

    #     # Append image PNG file name to base image path
    #     item_json['image'] = item_json['image'] + '/' + str(idx).zfill(zfill_count) + '.png'
        
    #     # Convert pandas series to dictionary
    #     attr_dict = dict(row)
        
    #     # Add all existing traits to attributes dictionary
    #     for attr in attr_dict:
            
    #         if attr_dict[attr] != 'none':
    #             item_json['pettributes'].append({ 'trait_type': attr, 'value': attr_dict[attr] })
        
    #     # Write file to json folder
    #     item_json_path = os.path.join(json_path, str(idx))
    #     with open(item_json_path, 'w') as f:
    #         json.dump(item_json, f)

# Run the main function
main()

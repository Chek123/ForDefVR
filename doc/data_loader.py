import pandas as pd
import numpy as np
import json

# EXCEL AND DATA PROCESSING

raw_data = pd.read_excel("ForDefVR.xlsx", sheet_name=["Level 1", "Level 2", "Level 3", "Level 4" ,"Level 5"] , usecols = "B:F")
df = pd.DataFrame()
df = df.append([raw_data['Level 1'], raw_data['Level 2'], raw_data['Level 3'], raw_data['Level 4'], raw_data['Level 5']])

df = df.drop([5,6,12,13,19,20,26,27,33], axis=0)
df = df.fillna('')

soldier_dict = {
	'': 0,
	'M4': 1,
	'AK': 2,
	'S': 3,
	'BA': 4,
	'M': 5,
	'B': 6
}

layout = df.values.tolist()
clean_data = []

for i in range(len(layout)):
	clean_data.extend(layout[i]) 

print("Lenght of clean data (should be 625): " + str(len(clean_data)))

# JSON PROCESSING

# load json template data
with open("data_template.json", "r") as json_read:
	data = json.load(json_read)

levels = data["number_of_levels"]

# updating data

cycle_counter = 0
flag = False

for level in range(levels):         # number of levels
	for grid in range(5):      		# 5 grids for each level
		for square in range(25):    #25 squares for each grid
			data["levels"][level]["grids"][grid]["square"][square]["vojak"] = soldier_dict[clean_data[cycle_counter]]
			cycle_counter += 1


print(str(cycle_counter) + " rows were updated (should be 625)")
# write data
with open("data.json", "w") as json_write:
    json.dump(data, json_write)
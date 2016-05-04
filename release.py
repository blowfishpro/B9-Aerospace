import os
import json
import zipfile
import re

def decode_version(file_name):
	f = open(file_name, 'r')
	data = json.load(f)
	f.close()
	
	version_info = data['VERSION']
	major = version_info['MAJOR']
	minor = version_info['MINOR']
	patch = version_info['PATCH']
	return '{}-{}-{}'.format(major, minor,patch)

def zipdir(path, ziph):
    # ziph is zipfile handle
    for root, dirs, files in os.walk(path):
        for file in files:
            ziph.write(os.path.join(root, file))

module_manager_list = [file_name for file_name in os.listdir('GameData') if re.match("ModuleManager[.\d]*dll", file_name)]

if len(module_manager_list) == 0:
	print("No ModuleManager found!")
	exit(1)
elif len(module_manager_list) > 1:
	print("More than one ModuleManager found:")
	for file_name in module_manager_list:
		print(file_name)
	exit(1)

module_manager_file = 'GameData/' + module_manager_list[0]
module_manager_readme = "GameData/ModuleManager_README.md"

b9_version = decode_version('GameData/B9_Aerospace/B9_Aerospace.version')
b9_legacy_version = decode_version('GameData/B9_Aerospace_Legacy/B9_Aerospace_Legacy.version')
b9_hx_version = decode_version('GameData/B9_Aerospace_HX/B9_Aerospace_HX.version')

print("Processing B9")

z = zipfile.ZipFile('B9_Aerospace_{}.zip'.format(b9_version), 'w', zipfile.ZIP_DEFLATED)
z.write('README.md')
z.write('CHANGELOG.md')
z.write(module_manager_file)
z.write(module_manager_readme)
zipdir('GameData/B9_Aerospace', z)
zipdir('GameData/B9AnimationModules', z)
zipdir('GameData/B9PartSwitch', z)
zipdir('GameData/Firespitter', z)
zipdir('GameData/JSI', z)
zipdir('GameData/Klockheed_Martian_Gimbal', z)
zipdir('GameData/SmokeScreen', z)
z.close()

print("Processing B9 Legacy")

z = zipfile.ZipFile('B9_Aerospace_Legacy_{}.zip'.format(b9_legacy_version), 'w', zipfile.ZIP_DEFLATED)
z.write('README.md')
z.write('CHANGELOG.md')
zipdir('GameData/B9_Aerospace_Legacy', z)
z.close()

print("Processing B9 HX")

z = zipfile.ZipFile('B9_Aerospace_HX_{}.zip'.format(b9_version), 'w', zipfile.ZIP_DEFLATED)
z.write('README.md')
z.write('CHANGELOG.md')
z.write(module_manager_file)
z.write(module_manager_readme)
zipdir('GameData/B9_Aerospace_HX', z)
zipdir('GameData/B9AnimationModules', z)
zipdir('GameData/B9PartSwitch', z)
zipdir('GameData/Klockheed_Martian_Gimbal', z)
zipdir('GameData/SmokeScreen', z)
z.close()

print("Done")
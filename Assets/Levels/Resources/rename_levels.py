import os
import re

regex_file_name = re.compile('(-?\s*)([^#\n]*)')
regex_level_name = re.compile('(\d*-?)(.*)')
i = 1
new_yaml = ''

with open('Levels.yaml', 'r') as f:
  for line in f:
    match_file_name = regex_file_name.search(line)
    if match_file_name is None: raise Exception('Cant find file name for `%s`' % line)
    
    # e.g. 02-Tutorial
    old_file_name = match_file_name.group(2)
    old_file_name = old_file_name.strip()
    
    match_level_name = regex_level_name.search(old_file_name)
    if match_level_name is None: raise Exception('Cant find level name for `%s`' % old_file_name)
    
    # e.g. Tutorial
    level_name = match_level_name.group(2)
    
    # e.g. 01-Tutorial
    new_file_name = '%02d-%s' % (i, level_name)
    i += 1
    
    os.rename(old_file_name + '.xml', new_file_name + '.xml')
    os.rename(old_file_name + '.xml.meta', new_file_name + '.xml.meta')
    
    new_yaml += line.replace(old_file_name, new_file_name)
    
    print '%-30s -> %s' % (old_file_name, new_file_name if old_file_name != new_file_name else 'unchanged')

with open('Levels.yaml', 'w') as f:
  f.write(new_yaml)

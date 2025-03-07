#!/usr/bin/env python

# Import wscript contents from build/tools/wscript.py

import os.path
from tools import wscript

# build temp directory
out = 'slag'

# build output directory
inst = 'output'

# How deep to scan for wscript_build files
maxdepth = 1

# appname should be the name of this build project, eg: 'Peach'
appname = 'Peach'

# Path to 3rdParty directory, eg: '3rdParty'
third_party = '3rdParty'

# Branch suffix to use as last portion of build tag
branch = 1

# blacklist wscripts inside any of the following subdirectories
ignore = []

# Returns true if the variant should be supported
def supported_variant(name):
	return name in [ 'win', 'linux', 'osx', 'doc' ]

def init(ctx):
	wscript.init(ctx)

def options(opt):
	wscript.options(opt)

def configure(ctx):
	wscript.configure(ctx)

def build(ctx):
	wscript.build(ctx)

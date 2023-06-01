Purpose of this project:

The idea is to create a model, which is capable to represent any kind of REST API (especially its structure) as a tree.
More precisely, every artifact that is part of the interface should be a node or a leaf within the tree structure and can be referenced over a XPath-like string, e.g.:

$.users.{userId}.GET.200.application/json.users[*].name

for referencing the payload property "name" that is embedded into the response payload of a queried user resource.
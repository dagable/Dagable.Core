<p align="center">
  <img src="./Documentation/images/logo.png" height="200px" />
</p>


![Twitter Follow](https://img.shields.io/twitter/follow/jwmxyz?label=%40jwmxyz&style=social)

![dagableCoreworkflow](https://github.com/dagable/Dagable.Core/actions/workflows/nugetBuildAndDeploy.yml/badge.svg)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/c09f67e58b27454abf4272366cc46bb2)](https://www.codacy.com?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Dagable/Dagable.Core&amp;utm_campaign=Badge_Grade)

## Dagable.Core

Dagable.Core is a nuget package that supports the creation and scheduling of Dynamic Acyclic Graphs.. 

- Installation
- Usage
- About
  - Generation
  - Scheduling
  - Sorting
- References

# Installation





# About

### Scheduling




### Sorting

For sorting the graph, *Kahn's algorithm* is used:


```
L ← Empty list that will contain the sorted elements
S ← Set of all nodes with no incoming edge

while S is not empty do
    remove a node n from S
    add n to L
    for each node m with an edge e from n to m do
        remove edge e from the graph
        if m has no other incoming edges then
            insert m into S

if graph has edges then
    return error   (graph has at least one cycle)
else 
    return L   (a topologically sorted order)
```

## References

> Tobita, Takao, and Hironori Kasahara. "A standard task graph set for fair evaluation of > multiprocessor scheduling algorithms." Journal of Scheduling 5.5 (2002): 379-394.

> Adam, Thomas L., K. Mani Chandy, and J. R. Dickson. "A comparison of list schedules for parallel processing systems." *Communications of the ACM* 17.12 (1974): 685-690.

> A. B. Kahn. 1962. Topological sorting of large networks. Commun. ACM 5, 11 (Nov. 1962), 558–562. DOI:https://doi.org/10.1145/368996.369025

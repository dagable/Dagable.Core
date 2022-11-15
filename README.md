<p align="center">
  <img src="./Documentation/images/logo.png" height="200px" />
</p>


![Twitter Follow](https://img.shields.io/twitter/follow/jwmxyz?label=%40jwmxyz&style=social)

![dagableCoreworkflow](https://github.com/dagable/Dagable.Core/actions/workflows/nugetBuildAndDeploy.yml/badge.svg)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/c09f67e58b27454abf4272366cc46bb2)](https://www.codacy.com?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Dagable/Dagable.Core&amp;utm_campaign=Badge_Grade)

# Dagable.Core

Dagable.Core is a nuget package that supports the creation and scheduling of Dynamic Acyclic Graphs. 

- [Installation](#Installation)
- [Usage](#Usage)
- [About](#About)
  - [Generation](#Generation)
  - [Scheduling](#Scheduling)
  - [Sorting](#Sorting)
- [References](#References)

## Installation

If you only require the standard/Critical Path task generation without the need for scheduling or critical paths you can just install the `Dagable.Core` nuget package found [here](https://www.nuget.org/packages/Dagable.Core/1.0.0-alpha-9).

If you require Scheduling install the nuget pacage found [here](https://www.nuget.org/packages/Dagable.Core.Scheduling/1.0.0-alpha-9).

Generation Only:

```dotnet add package Dagable.Core --version 1.0.0-alpha-9```

Scheduling and graph generation:

```dotnet add package Dagable.Core.Scheduling --version 1.0.0-alpha-9```

## Usage

Once installed you should register the services in your startup:

``` c#
public void ConfigureServices(IServiceCollection services)
{
    ...
        
  services
    .AddDagableCoreServices()
    .AddDagableSchedulingServices();
}
```

Once the services have been registed you can access the generation methods via the `IDagCreationService` interface and the scheduling methods via the `ITaskGraphSchedulingService`.

## About

### Generating

To generate random DAGs I use a Layer-by-Layer approach that builds a valid DAG.

Given N, the number of nodes, L the number of layers, P the probability of a connecting edge E being added, we can construct a random DAG using the following psuedo code.

```
generate(L, N, P)
	For i = 0 To L 
        layer <- RANDOM_INT(1, L)
        Add N with layer L to graph
    EndFor

    For Node in GraphNodes
        nextLayerNodes <- Graph Nodes with L == Node.L +1
        For NextNode in nextLayerNodes
            probability <- RANDOM_DOUBLE()
            If(probability <= P)
                Add E from Node to NextNode
            Endif
        EndFor
    EndFor

    //We then need to check all layered nodes have atleast one incoming edge
    nodesWithNoPredecessorNodes <- graphNodes with no pre Predecessor
    For Node in nodesWithNoPredecessorNodes
        prevLayer = Node.L - 1
        prevLayerNodes <- graphNodes with L == prevLayer
        selectedPredecessor <- RANDOM(prevLayerNodes)
        Add E from selectedPredecessor to Node
    EndFor
```



### Scheduling

#### Dynamic Level Scheduling Algorithm

The DLS algorithm  employs an attribute known as *dynamic level* (DL), which is the difference  between the static *b-level* (defined below) of a node and its earliest start time on a processor. In each of the steps when scheduling, the algorithm computes a DL for all nodes within the ready pool. In each of these steps, the node pair with the highest DL is selected for scheduling.

The DLS algorithm tends to schedule nodes in descending order of their static level at the start of the process. This seems to change towards the end of the process where nodes are assigned in ascending order of *t-level* (i.e. earliest start time), near the end of the scheduling process

The algorithm  is described below:

```
Calculate the b-level of each node.
Initially, the ready node pool holds only the entry nodes 
    
repeat

Calculate the earliest start time for every node on each of the 		processors. Then calculate the DL of every node-processor pair. This 	is calculated by subtracting the earliest start-time from the node 		static b-level
Schedule the node-processor pair which gives the largest DL to the corresponding processor. 
Add the newly ready nodes to the ready pool.

until all nodes have been scheduled.
```


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

> Kwok, Y.-K. and Ahmad, I. (1999b). Static scheduling algorithms for allocating directed task graphs to multiprocessors. ACM Computing Surveys (CSUR), 31(4):406–471

> Hagras, T. and Janecek, J. (2003). A high performance, low complexity algorithm for compile-time job scheduling in homogeneous computing environments. In Parallel Processing Workshops, 2003. Proceedings. 2003 International Conference on, pages 149–155. IEEE

> Sih, G. C. and Lee, E. A. (1993a). A compile-time scheduling heuristic for  interconnection-constrained heterogeneous processor architectures. IEEE transacttions on Parallel and Distributed systems, 4(2):175–187.
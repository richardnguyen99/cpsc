"""
Python program that simulates Mark-Sweep Garbage Collection algorithm
"""

from __future__ import annotations
from typing import List, Dict

__AUTHOR__ = "Richard Nguyen"


class MemoryObject(object):
    """
    Memory object simulates a heap block that contains a marked bit for Mark-Sweep
    algorithm, and references to other memory objects
    """

    def __init__(self, id: int):
        self.marked = False
        self.__id = id
        self.__references: List[MemoryObject] = []

    def referenceto(self, memobj: MemoryObject) -> None:
        """
        Append a new reference to which current Memory Object points
        """
        self.__references.append(memobj)

    def getid(self) -> int:
        return self.__id

    @staticmethod
    def mark(obj: MemoryObject) -> None:
        """
        Static method to mark all reachable objects from root pointers using
        recursive depth-first marking
        """
        if not obj.marked:
            obj.marked = True

            for refobj in obj.__references:
                MemoryObject.mark(refobj)


class Heap(object):
    """
    Heap to replicate virtual heap memory that contains variable pointers,
    allocated and reclaimed heap blocks.

    Heap must initialize data, with load(), before being used.
    """

    def __init__(self):
        self.__rootptrs: Dict[str, MemoryObject] = {}
        self.__heapblk: list[MemoryObject] = list()
        self.__reclaimblk: List[int] = list()
        self.__blkcnt: int = 0

    def __initheapblk(self) -> None:
        for i in range(self.__blkcnt):
            self.__heapblk.append(MemoryObject(i))

    def __parseref(self, line: str) -> None:
        line = line.strip()
        pointers = line.split(",")
        blkidx = int(pointers[1])

        memoryobj = self.__heapblk[blkidx]

        if pointers[0].isdigit():
            idx = int(pointers[0])
            self.__heapblk[idx].referenceto(memoryobj)
        else:
            self.__rootptrs[pointers[0]] = memoryobj

    def load(self, filename: str) -> None:
        """
        Load allocated heap blocks and variable pointers represented in filename
        """
        with open(filename, "r") as f:
            line = f.readline()
            self.__blkcnt = int(line.strip())

            self.__initheapblk()

            line = f.readline()
            while line:
                self.__parseref(line)

                line = f.readline()

    def mark(self) -> None:
        """
        Mark phase. Perform marking algorithm to keep track of all reachable
        heap blocks.

        A heap block is considered reachable when it can be accessed through
        a variable pointer
        """
        for rootptr in self.__rootptrs.keys():
            memobj = self.__rootptrs[rootptr]
            MemoryObject.mark(memobj)

    def sweep(self) -> None:
        """
        Sweep phase. Perform sweeping algorithm to remove all heap blocks that
        are unmarked from mark phase
        """
        self.__reclaimblk.clear()

        for memobj in self.__heapblk:
            if memobj.marked:
                memobj.marked = False
            else:
                self.__reclaimblk.append(memobj.getid())
                self.__heapblk[self.__heapblk.index(memobj)] = None
                self.__blkcnt -= 1

        self.__heapblk = [block for block in self.__heapblk if block]

        print(
            f"Marked nodes: {' '.join([str(block.getid()) for block in self.__heapblk])}")
        print(
            f"Reclaimed nodes: {' '.join([str(blockid) for blockid in self.__reclaimblk])}")


def collect(filename: str) -> None:
    heap = Heap()
    heap.load(filename)

    heap.mark()
    heap.sweep()


def _main(filename: str) -> None:
    collect(filename)

    print()


if __name__ == '__main__':
    filename = "sample2.txt"

    _main(filename)

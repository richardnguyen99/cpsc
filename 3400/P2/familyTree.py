"""
GEDCOM parser design

Create empty dictionaries of individuals and families
Ask user for a file name and open the gedcom file
Read a line
Skips lines until a FAM or INDI tag is found
    Call functions to prcess those two types

Processing an Individual
Get pointer string
Make dictionary entry for pointer with ref to Perspn object
Find name and identify parts (surname, given names, suffix)
Find FAMS and FAMC tags; store FAM references for later linkage
Skip other lines

Processing a family
Get pointer string
Make dictionary entry for pointer with ref to Family object
Find HUSB WIFE and CHIL tags
    Add included pointers to Family objects
Skip other lines

Testing to show the data structures:
    Print info from the collections of Person and Family objects
    Print descendant chart after all lines are processed

"""

from __future__ import annotations
from typing import Dict, List, Tuple
from collections import namedtuple
__author__ = "Richard Nguyen"
__version__ = "0.1"


class Person():
    """
    Person() stores infomation about a single person such as their name, sex, and relationship with other person

    Person should be proccessed when an INDI tag is found
    """

    def __init__(self, ref: str):
        self._id: str = ref
        self._asSpouse: List[str] = []
        self._asChild: str = None

    def addName(self, names: List[str]) -> None:
        """
        Add a complete name to an existing Person record

        {names} should be an array of given name(0), surname(1) and suffix(2)
        """

        self._given = names[0]
        self._surname = names[1]
        self._suffix = names[2]

    def addIsSpouse(self, famRef) -> None:
        """
        Add {famRef} to indicate families in which this person is a spouse

        A person can be a spouse in many families.
        """

        self._asSpouse.append(famRef)

    def addIsChild(self, famRef) -> None:
        """
        Store {famRef} to indicate family in which this person is a child

        A person can be only a child to a family
        """

        self._asChild = famRef

    def printDescendants(self, prefix='') -> None:
        """
        Print info for this Person and the descendants in families where
        this Person is spouse to
        """

        print(prefix + self.name())

        for fam in self._asSpouse:
            families[fam].printFamily(self._id, prefix)

    def name(self) -> str:
        """
        Return a representation of this Person's name
        """

        return self._given + ' ' + self._surname.upper() + ' ' + self._suffix

    def treeInfo(self) -> str:
        """
        Return a string representation of the structure in which the Person is included
        """
        childString = ' | asChild: ' + self._asChild if self._asChild else ' '
        spouseString = ' | asSpouse: ' + \
            ','.join(self._asSpouse) if self._asSpouse else ' '

        return childString + spouseString

    # TODO: implement me
    def eventInfo(self) -> str:
        """
        Return a string representation of events once they are recognized
        """
        return ''

    @staticmethod
    def getPerson(personRef: str) -> Person:
        """
        Return a Person in the global record of Person's
        """

        return persons[personRef]

    def isDescendant(self, personID: str) -> bool:
        """
        Check if the Person is a descendant of person with {personID}

        Return True if it's the same person, or descendant is true. Otherwise,
        return False if the Person is no child to any family descendant is false,
        or no parent is found.
        """
        if (self._id == personID):
            return True

        if self._asChild is None:
            return False

        parents = families[self._asChild]

        if parents is None:
            return False

        if parents._spouse1.personRef == personID or parents._spouse2.personRef == personID:
            return True

        if Person.getPerson(parents._spouse1.personRef).isDescendant(personID) or Person.getPerson(parents._spouse2.personRef).isDescendant(personID):
            return True

        return False

    def __str__(self) -> str:
        """
        Return a string representation of the Person's information such as name, event and family tree
        """
        return self.name() + self.eventInfo() + self.treeInfo()


Spouse = namedtuple('Spouse', ['personRef', 'tag'])


class Family:

    def __init__(self, ref):

        self._id: str = ref
        self._spouse1: Spouse = None
        self._spouse2: Spouse = None
        self._children: List[str] = []

    def addSpouse(self, personRef: str, tag: str) -> None:
        """
        Store {personRef} to identify a person who is a spouse to the Family

        For simplicity, spouse1 will be always added first, then spouse2. So, client must keep track of the order
        """
        newSpouse = Spouse(personRef, tag)
        if (self._spouse1) == None:
            self._spouse1 = newSpouse
        else:
            self._spouse2 = newSpouse

    def addChild(self, personRef: str) -> None:
        """
        Add {personRef} to indicate a person who is a child in the Family
        """

        self._children.append(personRef)

    def printFamily(self, firstSpouse, prefix) -> None:
        """
        Display the Family tree

        This method can be used alone or in Person.printDescendants() to print
        the entire family tree according to a Person
        """

        if prefix != '':
            prefix = prefix[:-2] + '  '

        if self._spouse2:
            if self._spouse1.personRef == firstSpouse:
                secondSpouse = self._spouse2.personRef
            else:
                secondSpouse = self._spouse1.personRef

            print(prefix + '+' + persons[secondSpouse].name())

        for child in self._children:
            persons[child].printDescendants(prefix+'|--')

    def __str__(self):
        """
        Return a string representation of all the information in the Family
        """
        spousePart = ''

        for spouse in (self._spouse1, self._spouse2):
            if spouse:
                if spouse.tag == 'HUSB':
                    spousePart += ' Husband: ' + spouse.personRef
                else:
                    spousePart += ' Wife: ' + spouse.personRef
        childrenPart = '' if self._children == [
        ] else ' Children: ' + ','.join(self._children)

        return spousePart + childrenPart


persons: Dict[str, Person] = dict()
families: Dict[str, Family] = dict()


def getPerson(personID):
    return persons[personID]


def getFamily(familyID):
    return families[familyID]


def printAllPersonInfo():
    # Print out all information stored about individuals
    for ref in sorted(persons.keys()):
        print(ref + ':' + str(persons[ref]))
    print()


def printAllFamilyInfo():
    # Print out all information stored about families
    for ref in sorted(families.keys()):
        print(ref + ':' + str(families[ref]))
    print()


def processGEDCOM(file) -> None:

    def getPointer(line: str) -> str:
        return line[8:].split('@')[0]

    def processPerson(newPerson: Person) -> None:
        nonlocal line
        line = f.readline()
        while line[0] != '0':
            tag = line[2:6]

            if tag == 'NAME':
                names = line[6:].split('/')

                names[0] = names[0].strip()
                names[2] = names[2].strip()
                newPerson.addName(names)

            elif tag == 'FAMS':
                newPerson.addIsSpouse(getPointer(line))
            elif tag == 'FAMC':
                newPerson.addIsChild(getPointer(line))

            line = f.readline()

    def processFamily(newFamily: Family) -> None:
        nonlocal line
        line = f.readline()

        while line[0] != '0':
            tag = line[2:6]

            if tag == 'HUSB':
                newFamily.addSpouse(getPointer(line), 'HUSB')
            elif tag == 'WIFE':
                newFamily.addSpouse(getPointer(line), 'WIFE')
            elif tag == 'CHIL':
                newFamily.addChild(getPointer(line))

            line = f.readline()

    f = open(file, 'r')
    line = f.readline()

    while line != '':
        fields = line.strip().split(' ')

        if line[0] == '0' and len(fields) > 2:
            if (fields[2] == 'INDI'):
                ref = fields[1].strip('@')
                persons[ref] = Person(ref)
                processPerson(persons[ref])

            elif (fields[2] == 'FAM'):
                ref = fields[1].strip('@')
                families[ref] = Family(ref)
                processFamily(families[ref])

            else:
                line = f.readline()
        else:
            line = f.readline()


def main():
    filename = "gedcom/Kennedy.ged"  # Set a default name for the file to be processed
# Uncomment the next line to make the program interactive
##    filename = input("Type the name of the GEDCOM file:")

    processGEDCOM(filename)

    printAllPersonInfo()

    printAllFamilyInfo()

    person = "I46"  # Default selection to work with Kennedy.ged file
# Uncomment the next line to make the program interactive
##    person = input("Enter person ID for descendants chart:")

    getPerson(person).printDescendants()
    # print(getPerson(person).isDescendant("I45"))


if __name__ == '__main__':
    main()

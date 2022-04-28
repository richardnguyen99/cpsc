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
from typing import Deque, Dict, List
from collections import deque, namedtuple
__author__ = "Richard Nguyen"
__version__ = "0.5"


class Event():
    """
    An Event instance is associated with one Person by referencing Person ID.
    """

    def __init__(self, personRef):
        self._id = personRef
        self._birthDate = ''
        self._birthPlace = ''
        self._deathDate = ''
        self._deathPlace = ''
        self._burriedAt = ''
        self._marriedTo = ''
        self._marriedDate = ''
        self._marriedPlace = ''

    def addMarriageDate(self, marriageDate: str) -> None:
        """
        Set marriage date Event of Person according to Person ID
        """

        self._marriedDate = marriageDate

    def addMarriagePlace(self, marriagePlace: str) -> None:
        """
        Set marriage place Event of Person according to Person ID
        """

        self._marriedPlace = marriagePlace

    def addBirthDate(self, birthDate: str) -> None:
        """
        Set birth date Event of Person according to Person ID
        """

        self._birthDate = birthDate

    def addBirthPlace(self, birthPlace: str) -> None:
        """
        Set birth place Event of Person according to Person ID
        """

        self._birthPlace = birthPlace

    def addDeathDate(self, deathDate: str) -> None:
        """
        Set death place Event of Person according to Person ID
        """

        self._deathDate = deathDate

    def addDeathPlace(self, deathPlace: str) -> None:
        """
        Set death place Event of Person according to Person ID
        """

        self._deathPlace = deathPlace

    def birth(self) -> str:
        """
        Print string representation for birth event of a Person
        """

        if self._birthDate or self._birthPlace:
            return f"n {self._birthDate} {self._birthPlace}"

        return ''

    def death(self) -> str:
        """
        Print string representation for death event of a Person
        """

        if self._deathDate or self._deathPlace:
            return f"d {self._deathDate} {self._deathPlace}"

        return ''

    def marriage(self) -> str:
        """
        Print string representation for marriage event of a Person
        """

        if self._marriedDate or self._marriedPlace:
            return f"m {self._marriedDate} {self._marriedPlace}"

        return ''

    def __str__(self) -> str:
        """
        Return string representation for all events of a Person
        """

        return ', '.join(filter(None, [self.birth(), self.marriage(), self.death()]))

    @staticmethod
    def getEvent(personRef: str) -> Event:
        """
        Get an Event for a person from global list of events
        """

        return events[personRef]


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

        print(prefix + str(self))

        for fam in self._asSpouse:
            families[fam].printFamily(self._id, prefix)

    def printAncestors(self, height=0) -> None:
        """
        Print info for this Person and the ancestors in families to which
        this Person is a descendant.
        """

        indentSize: str = "  " * height

        if self._asChild is None:
            print(f"{indentSize}{height} {self.name()}{self.eventInfo()}")
            return

        parents = families[self._asChild]

        if parents is None:
            return

        # The idea behind this function is to use in-order traversal in binary
        # tree so we don't need to put extra functions to support this method.

        # Print out ancestors from one parent's side
        Person.getPerson(parents._spouse1.personRef).printAncestors(height + 1)

        # Print out self
        print(f"{indentSize}{height} {self.name()}{self.eventInfo()}")

        # Print out ancestors from the other parent's side
        Person.getPerson(parents._spouse2.personRef).printAncestors(height + 1)

    def printCousins(self, nth=1) -> None:
        """
        Print out all the nth cousins of the Person.

        Nth cousins are considered as grandchildren of Nth (great) grandparents
        """

        # wrong user input
        if nth < 1:
            print(f"Wrong ordinal number: {nth}")
            print("\tNo cousins")
            return

        # Everything after 3 should be replace with th suffix
        ordinalString = ("First", "Second", "Third")
        print(
            f"{ordinalString[nth] if nth < 4 else f'{nth}th'} counsins for {self.name()}")

        # If the Person is the root of the family tree
        if self._asChild is None:
            print("\tNo cousins")
            return

        # 1 represents the height between the Person and the parent
        # while nth is the height to the nth grandparent
        height = nth + 1
        familyQueue: Deque[Family] = deque()

        # Ignore presenting family refs.
        # I.e. First grandparent ignores the Person family since there is no cousin
        ignoreFamilyRefs: Deque[str] = deque()

        family = families[self._asChild]
        ignoreFamilyRefs.append(self._asChild)

        # Parents' side
        spouse1 = Person.getPerson(family._spouse1.personRef)
        spouse2 = Person.getPerson(family._spouse2.personRef)

        # Parents' families
        if spouse1._asChild is not None:
            familyQueue.append(families[spouse1._asChild])
        if spouse2._asChild is not None:
            familyQueue.append(families[spouse2._asChild])

        if familyQueue is None:
            print("\tNo cousins")

        # At this point there are two families, each is where
        # one parent is from. if n > 1, go up to the families
        # where families in familyQueue are belonged to.
        # Those existing families in familyQueue will be put
        # in a blacklist so no wrong order of cousin is printed
        heightCount = 2
        while heightCount < height:
            length = len(familyQueue)

            for _ in range(length):
                family = familyQueue.popleft()
                spouse1 = Person.getPerson(family._spouse1.personRef)
                spouse2 = Person.getPerson(family._spouse2.personRef)

                familyQueue.append(families[spouse1._asChild])
                familyQueue.append(families[spouse2._asChild])
                ignoreFamilyRefs.append(spouse1._asChild)
                ignoreFamilyRefs.append(spouse2._asChild)

            heightCount += 1

        # At the point, familyQueue contains the families where the correct nth
        # grandparents are spoused to. Continue on append more sub-families to
        # familyQueue, according to the current family in familyQueue.
        while heightCount > 1:
            # Breadth-first search where heightCount maintains the height of the tree
            # Length represents how many families there are in the same height.
            length = len(familyQueue)

            for _ in range(length):
                family = familyQueue.popleft()

                for i in range(len(family._children)):
                    childRef = family._children[i]
                    child = Person.getPerson(childRef)

                    for fam in child._asSpouse:
                        familyQueue.append(families[fam])

            heightCount -= 1

        # Here, familyQueue should contain only nth cousins. `hasCounsins` is a flag
        # to check if the Person has at least one nth cousin.
        hasCousins = False
        while familyQueue:
            family = familyQueue.popleft()
            if family._id in ignoreFamilyRefs:
                continue

            for children in family._children:
                hasCousins = True
                child = Person.getPerson(children)
                print(f"\t{child}")

        if not hasCousins:
            print("\tNo cousins")

    def name(self) -> str:
        """
        Return a representation of this Person's name
        """

        return " ".join(filter(None, [self._given, self._surname.upper(), self._suffix]))

    def treeInfo(self) -> str:
        """
        Return a string representation of the structure in which the Person is included
        """
        childString = f" | asChild: {self._asChild if self._asChild else ' '}"
        spouseString = f" | asSpouse: {','.join(self._asSpouse) if self._asSpouse else ' '}"

        return childString + spouseString

    # TODO: implement me
    def eventInfo(self) -> str:
        """
        Return a string representation of events once they are recognized
        """
        return Event.getEvent(self._id).__str__()

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

        for family in self._asSpouse:
            for childRef in families[family]._children:
                child = Person.getPerson(childRef)
                if child.isDescendant(personID):
                    return True

        return False

    def __str__(self) -> str:
        """
        Return a string representation of the Person's information such as name, event and family tree
        """
        return f"{self.name()}, {self.eventInfo()}{self.treeInfo()}"


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

            print(prefix + '+' + str(persons[secondSpouse]))

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
events: Dict[str, Event] = dict()


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
        option = ''

        newEvent = events[newPerson._id]

        methods = {
            "BIRT": {
                "DATE": newEvent.addBirthDate,
                "PLAC": newEvent.addBirthPlace
            },
            "DEAT": {
                "DATE": newEvent.addDeathDate,
                "PLAC": newEvent.addDeathPlace
            },
            "MARR": {
                "DATE": newEvent.addMarriageDate,
                "PLAC": newEvent.addMarriagePlace
            }
        }
        while line[0] != '0':
            tag = line[2:6]

            if line[0] == '1':
                option = tag

            if tag == 'NAME':
                names = line[6:].split('/')

                names[0] = names[0].strip()
                names[2] = names[2].strip()
                newPerson.addName(names)
            elif (tag == 'DATE' or tag == 'PLAC') and (option == 'BIRT' or option == 'DEAT'):
                methods[option][tag](line[6:].strip())
            elif tag == 'FAMS':
                newPerson.addIsSpouse(getPointer(line))
            elif tag == 'FAMC':
                newPerson.addIsChild(getPointer(line))

            line = f.readline()

    def processFamily(newFamily: Family) -> None:
        nonlocal line
        line = f.readline()
        husbandRef = ''
        wifeRef = ''
        while line[0] != '0':
            tag = line[2:6]

            if line[0] == '1':
                option = tag

            if tag == 'HUSB':
                newFamily.addSpouse(getPointer(line), 'HUSB')
                husbandRef = getPointer(line)
            elif tag == 'WIFE':
                newFamily.addSpouse(getPointer(line), 'WIFE')
                wifeRef = getPointer(line)
            elif tag == 'CHIL':
                newFamily.addChild(getPointer(line))
            elif option == 'MARR' and (tag == 'DATE' or tag == 'PLAC'):
                if tag == 'DATE':
                    if husbandRef:
                        events[husbandRef].addMarriageDate(line[6:].strip())
                    if wifeRef:
                        events[wifeRef].addMarriageDate(line[6:].strip())
                elif tag == 'PLAC':
                    if husbandRef:
                        events[husbandRef].addMarriagePlace(line[6:].strip())
                    if wifeRef:
                        events[wifeRef].addMarriagePlace(line[6:].strip())

            line = f.readline()

    f = open(file, 'r')
    line = f.readline()

    while line != '':
        fields = line.strip().split(' ')

        if line[0] == '0' and len(fields) > 2:
            if (fields[2] == 'INDI'):
                ref = fields[1].strip('@')
                persons[ref] = Person(ref)
                events[ref] = Event(ref)
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
    filename = "Kennedy.ged"  # Set a default name for the file to be processed
# Uncomment the next line to make the program interactive
# filename = input("Type the name of the GEDCOM file:")

    processGEDCOM(filename)

    printAllPersonInfo()

    printAllFamilyInfo()

    person = "I55"  # Default selection to work with Kennedy.ged file
# Uncomment the next line to make the program interactive
# person = input("Enter person ID for descendants chart:")

    # getPerson('I1').printDescendants()
    # print(getPerson(person).isDescendant("I45"))
    # getPerson("I54").printAncestors()
    print()

    getPerson('I81').printCousins(2)


if __name__ == '__main__':
    main()

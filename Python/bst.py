class node:

    def __init__(self, key, data, left=None, right=None):
        self.key = key
        self.data = data
        self.left = left
        self.right = right

    def updateLeftLink(self, link):
        self.left = link

    def updateRightLink(self, link):
        self.right = link

    def getElement(self):
        return self.key

    def getLeftNode(self):
        return self.left

    def getRightNode(self):
        return self.right


class BST:
    def __init__(self):
        self.root = None
        self.pHeight = 0

    def isEmpty(self):
        return self.root is None

    def getRoot(self):
        return self.root

    def insert(self, key):
        tempNode = node(key)

        if self.getRoot() is None:
            self.root = tempNode
        else:
            inserted = False
            p = self.root
            while not inserted:
                # if new element is greater the current element then insert it to the right
                if tempNode.getElement() > p.getElement():
                    # if right link of the current is null then directly insert
                    if p.getRightNode() is None:
                        p.updateRightLink(tempNode)
                        inserted = True
                    else:
                        p = p.getRightNode()

                # if new element is less the current element then insert it to the left
                elif tempNode.getElement() < p.getElement():
                    # if left link of the current is null then directly insert
                    if p.getLeftNode() is None:
                        p.updateLeftLink(tempNode)
                        inserted = True
                    else:
                        p = p.getLeftNode()

    # recursive method to display the binary tree in inorder
    def displayInorder(self, tempNode):
        if tempNode is None:
            return
        else:
            self.displayInorder(tempNode.getLeftNode())
            print(tempNode.getElement(), end="  ")
            self.displayInorder(tempNode.getRightNode())

    # recursive method to display the binary tree in preorder
    def displayPreorder(self, tempNode):
        if tempNode is None:
            return
        else:
            print(tempNode.getElement(), end="  ")
            self.displayPreorder(tempNode.getLeftNode())
            self.displayPreorder(tempNode.getRightNode())

    # recursive method to display binary tree in postorder
    def displayPostorder(self, tempNode):
        if tempNode is None:
            return
        else:
            self.displayPostorder(tempNode.getLeftNode())
            self.displayPostorder(tempNode.getRightNode())
            print(tempNode.getElement(), end="  ")

    # method to search an element in the binary search tree
    # if found method returns true
    def search(self, element):

        # if the bst is empty return false
        if self.isEmpty():
            return False

        tempNode = self.getRoot()
        found = False

        while not found:

            # if tempNode is null then the element is not present
            # break out of the loop
            if tempNode is None:
                break

            # if the element are less than current data traverse right
            if tempNode.getElement() > element:
                tempNode = tempNode.getLeftNode()

            # if the element are greater than current data traverse left
            elif tempNode.getElement() < element:
                tempNode = tempNode.getRightNode()

            # if the element is equal to the current data then set found to true
            elif tempNode.getElement() == element:
                found = True

        return found


# main function

b1 = BST()
b1.insert(7)
b1.insert(29)
b1.insert(25)
b1.insert(36)
b1.insert(71)
b1.insert(24)
b1.insert(5)
b1.insert(9)
b1.insert(1)
root = b1.getRoot()

print("\n\nThe Inorder traversal of the BST is : ", end=" ")
b1.displayInorder(root)

print("\n\nThe Preorder traversal of the BST is : ", end=" ")
b1.displayPreorder(root)

print("\n\nThe Postorder traversal of the BST is : ", end=" ")
b1.displayPostorder(root)

print("\n")
# searching for the elements in the binary search tree
print(b1.search(25))
print(b1.search(37))

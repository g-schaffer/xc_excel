def string_to_hex(s):
    """
    Convert a string to its hexadecimal representation.

    Args:
        s (str): The input string.

    Returns:
        str: The hexadecimal representation of the input string.
    """
    return s.encode("utf-8").hex()

def hex_to_string(h):
    """
    Convert a hexadecimal representation back to its string form.

    Args:
        h (str): The hexadecimal string.

    Returns:
        str: The original string.
    """
    return bytes.fromhex(h).decode("utf-8")

# Exemple d'utilisation
if __name__ == "__main__":
    original_string = """
from __future__ import print_function
from __future__ import division

import warnings


warnings.filterwarnings("ignore")


import geom
import xc
from model import predefined_spaces
from materials import typical_materials


L = 1.0  
E = 210e9  
alpha = 1.2e-5  
A = 4e-4  
AT = 10  


feProblem = xc.FEProblem ()
preprocessor = feProblem.getPreprocessor


nodes = preprocessor.getNodeHandler
modelSpace = predefined_spaces.SolidMechanics2D(nodes)
nod1 = nodes.newNodeXY(0.0 ,0.0)
nod2 = nodes.newNodeXY(L ,0.0)
elast = typical_materials.defElasticMaterial(preprocessor, "elast", E)
elements = preprocessor.getElementHandler
elements.defaultMaterial = "elast"
elements.dimElem = 2  
truss = elements.newElement("Truss", xc.ID([nod1.tag, nod2.tag]))
truss.sectionArea= A


constraints = preprocessor.getBoundaryCondHandler
spc1 = constraints.newSPConstraint(nod1.tag, 0, 0.0)
spc2 = constraints.newSPConstraint(nod1.tag, 1, 0.0)
spc3 = constraints.newSPConstraint(nod2.tag, 0, 0.0)
spc4 = constraints.newSPConstraint(nod2.tag, 1, 0.0)


loadHandler = preprocessor.getLoadHandler
lPatterns = loadHandler.getLoadPatterns
ts = lPatterns.newTimeSeries("linear_ts", "ts")
lPatterns.currentTimeSeries = "ts"
lp0 = lPatterns.newLoadPattern("default", "0")
eleLoad = lp0.newElementalLoad("truss_strain_load")
eleLoad.elementTags = xc.ID([truss.tag])
eleLoad.eps1 = alpha * AT
eleLoad.eps2 = alpha * AT
lPatterns.addToDomain("0")


result= modelSpace.analyze(calculateNodalReactions= False)


elem1 = elements.getElement(truss.tag)
elem1.getResistingForce()
N = elem1.getN()
print('N= '+str(N))

"""
    hex_string = string_to_hex(original_string)
    print(f"En hexadécimal: {hex_string}")

    recovered_string = hex_to_string(hex_string)
    print(f"Récupéré de l'hexadécimal: {recovered_string}")

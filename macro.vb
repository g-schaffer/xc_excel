Sub RunDockerCommand()
    Dim scrpt As String
    Dim command As String
    Dim result As String
    Dim shell As Object
    
    scrpt = "from __future__ import print_function\n"
    scrpt = scrpt & "from __future__ import division\n"
    scrpt = scrpt & "import warnings\n"
    scrpt = scrpt & "warnings.filterwarnings('ignore')\n"
    scrpt = scrpt & "import geom\n"
    scrpt = scrpt & "import xc\n"
    scrpt = scrpt & "from model import predefined_spaces\n"
    scrpt = scrpt & "from materials import typical_materials\n"
    scrpt = scrpt & "L = 1.0\n"
    scrpt = scrpt & "E = 210e9\n"
    scrpt = scrpt & "alpha = 1.2e-5\n"
    scrpt = scrpt & "A = 4e-4\n"
    scrpt = scrpt & "AT = 10\n"
    scrpt = scrpt & "feProblem = xc.FEProblem()\n"
    scrpt = scrpt & "preprocessor = feProblem.getPreprocessor\n"
    scrpt = scrpt & "nodes = preprocessor.getNodeHandler\n"
    scrpt = scrpt & "modelSpace = predefined_spaces.SolidMechanics2D(nodes)\n"
    scrpt = scrpt & "nod1 = nodes.newNodeXY(0.0,0.0)\n"
    scrpt = scrpt & "nod2 = nodes.newNodeXY(L,0.0)\n"
    scrpt = scrpt & "elast = typical_materials.defElasticMaterial(preprocessor, 'elast', E)\n"
    scrpt = scrpt & "elements = preprocessor.getElementHandler\n"
    scrpt = scrpt & "elements.defaultMaterial = 'elast'\n"
    scrpt = scrpt & "elements.dimElem = 2\n"
    scrpt = scrpt & "truss = elements.newElement('Truss', xc.ID([nod1.tag, nod2.tag]))\n"
    scrpt = scrpt & "truss.sectionArea= A\n"
    scrpt = scrpt & "constraints = preprocessor.getBoundaryCondHandler\n"
    scrpt = scrpt & "spc1 = constraints.newSPConstraint(nod1.tag, 0, 0.0)\n"
    scrpt = scrpt & "spc2 = constraints.newSPConstraint(nod1.tag, 1, 0.0)\n"
    scrpt = scrpt & "spc3 = constraints.newSPConstraint(nod2.tag, 0, 0.0)\n"
    scrpt = scrpt & "spc4 = constraints.newSPConstraint(nod2.tag, 1, 0.0)\n"
    scrpt = scrpt & "loadHandler = preprocessor.getLoadHandler\n"
    scrpt = scrpt & "lPatterns = loadHandler.getLoadPatterns\n"
    scrpt = scrpt & "ts = lPatterns.newTimeSeries('linear_ts', 'ts')\n"
    scrpt = scrpt & "lPatterns.currentTimeSeries = 'ts'\n"
    scrpt = scrpt & "lp0 = lPatterns.newLoadPattern('default', '0')\n"
    scrpt = scrpt & "eleLoad = lp0.newElementalLoad('truss_strain_load')\n"
    scrpt = scrpt & "eleLoad.elementTags = xc.ID([truss.tag])\n"
    scrpt = scrpt & "eleLoad.eps1 = alpha * AT\n"
    scrpt = scrpt & "eleLoad.eps2 = alpha * AT\n"
    scrpt = scrpt & "lPatterns.addToDomain('0')\n"
    scrpt = scrpt & "result= modelSpace.analyze(calculateNodalReactions=False)\n"
    scrpt = scrpt & "elem1 = elements.getElement(truss.tag)\n"
    scrpt = scrpt & "elem1.getResistingForce()\n"
    scrpt = scrpt & "N = elem1.getN()\n"
    scrpt = scrpt & "print('N= '+str(N))"
    
    ' Créez un objet WScript.Shell
    Set shell = CreateObject("WScript.Shell")
    
    ' Concaténation de la commande avec la variable hexadécimale
    command = "powershell -command ""docker run my:latest python driver.py " & StringToHex(scrpt) & """"
    
    result = shell.Exec(command).StdOut.ReadAll

    ' Affichez le résultat dans une boîte de message
    MsgBox result

    ' Libérez l'objet shell
    Set shell = Nothing
    Exit Sub
End Sub

Function StringToHex(s As String) As String
    Dim i As Integer
    Dim hexString As String
    Dim charCode As String
    
    hexString = ""
    
    For i = 1 To Len(s)
        charCode = Hex(Asc(Mid(s, i, 1)))
        If Len(charCode) = 1 Then
            charCode = "0" & charCode
        End If
        hexString = hexString & charCode
    Next i
    
    StringToHex = hexString
End Function


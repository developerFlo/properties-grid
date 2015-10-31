# properties-grid

PropertiesGrid is a WPF UserControl to show 3 dimensional data on a table. A data structure which fits to this control looks like:

- Row[]
 - Row[0].Column[]
   - Row[0].Column[0]
     - .Property1
      - .Property2
      -  ...

The main aspect lies on **high customization** with the **best possible performance**.

The repository is splitted into two projects:
 1. The main **PropertiesGrid** to be inclued from an application
 2. The **PropertiesGridSample** wich contains some static sample data to _show the functionality und usage_ of the grid.

##Features

- fixed column- und row(property)-headers
- high performance with virtualization
- functional base for inline edit per cell
- hover management
- high customization with templates for:
 - Column-Head
 - Row-Head
 - Property-Head
 - Cells per Property

##Sample

|               |               | Col 1 | Col 2 | Col 3 |
| ------------- |:-------------:| -----:| -----:| -----:|
| Row 1         | Property 1    | Value | Value | ...   |
|               | Property 2    | Value | ...   |       |
|               | Property 3    | ...   |       |       |
| Row 2         | Property 1    |       |       |       |
|               | Property 2    |       |       |       |
|               | Property 3    |       |       |       |
| Row 3         | Property 1    |       |       |       |
| ...           | ...           |       |       |       |

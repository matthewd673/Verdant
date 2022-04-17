# Verdant MapTools

Build compact, highly customizable, easy-to-parse map files from images.

## Usage

- MapTools takes PNG, BMP, or JPG files as input and writes a TXT file as output.
- `.\MapTools [input file] [.mapdict file] [output file]`.
- If no MAPDICT path is specified, MapTools will attempt to load the file at `default.mapdict`.
- If no output path is specified, MapTools will write to `output.txt`
- If a folder is given as input, each valid image file will be converted and written to the specified output path (where the filename is replaced with the input file's name).

## .mapdict

- MAPDICT files map color values (input) to character values (output).
- Notation: `r,g,b=c` *(for example: `255,0,50=a`)*.
- `\n` is the only reserved character.
- Lines beginning with `#` will be ignored (though `#` can still be used as an output character).

## Parsing

- MapTools output is designed to be easy to parse with a custom implementation.
- The first line of the file contains the width and height of the map, separated by a comma.
- The second line of the file contains a sequence of characters, each representing one tile of the map.

**Example Map:**

`[width],[height]
aaaaabbaaaaccccccccccaabbbbbba`

*(where `w` represents a wall and `.` represents a floor tile):*
```
24,24
wwwwwwwwwwwwwwwwwwwwwwwwwwww................wwwwwwww................wwwwwwww................wwwww......................ww......................ww.....w..........w.....ww.....w...wwww...w.....ww.....ww.w....w.ww.....ww.....www......www.....ww......................ww......................ww......................ww......................ww.....www......www.....ww.....ww.w....w.ww.....ww.....w...wwww...w.....ww.....w..........w.....ww......................ww......................wwwww................wwwwwwww................wwwwwwww................wwwwwwwwwwwwwwwwwwwwwwwwwwww
```
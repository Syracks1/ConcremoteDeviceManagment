﻿SELECT bas_art_nr, description, amount, assembly_order
FROM pricelist, Device_Parts, Step_Parts
WHERE Step_Parts.Step_id = 7;
/* dit is een test om te kijken of GitHub nog werkt */
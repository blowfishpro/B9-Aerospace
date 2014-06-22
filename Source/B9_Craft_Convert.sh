#!/bin/bash

cd "$PWD"

BAKDATE=.bak$(date +%Y%m%d)

shopt -s nullglob

deprecate () {
    echo -n "Converting $1 .."
    sed -e "s/B9\.Utility\.Light\.N2\.Red/B9.Utility.Light.N1.White/g" \
        -e "s/B9\.Utility\.Light\.N2\.Green/B9.Utility.Light.N1.White/g" \
        -e "s/B9\.Utility\.Landing\.Gear\.HDG1T/B9.Utility.Landing.Gear.HDG1A/g" \
        -e "s/B9\.Utility\.Landing\.Gear\.HDGTL/B9.Utility.Landing.Gear.HDG1AL/g" \
        -e "s/B9\.Utility\.Landing\.Gear\.HDG2T/B9.Utility.Landing.Gear.HDG2A/g" \
        -e "s/B9\.Utility\.Landing\.Gear\.HDG2TL/B9.Utility.Landing.Gear.HDG2AL/g" \
        -e "s/B9\.Control\.RCS\.Block\.R5T/B9.Control.RCS.Block.R5/g" \
        -e "s/B9\.Control\.RCS\.Block\.R12T/B9.Control.RCS.Block.R12/g" \
        -e "s/B9\.Control\.RCS\.Port\.R1T/B9.Control.RCS.Port.R1/g" \
        -e "s/B9\.Cockpit\.S2\.Body\.Fuel/B9.Cockpit.S2.Body/g" \
        -e "s/B9\.Cockpit\.S2\.Body\.LFO/B9.Cockpit.S2.Body/g" \
        -e "s/B9\.Cockpit\.MK2\.Body\.Fuel\.2m/B9.Cockpit.MK2.Body.2m/g" \
        -e "s/B9\.Cockpit\.MK2\.Body\.LFO\.2m/B9.Cockpit.MK2.Body.2m/g" \
        -e "s/B9\.Cockpit\.MK2\.Body\.Fuel\.5m/B9.Cockpit.MK2.Body.5m/g" \
        -e "s/B9\.Cockpit\.MK2\.Body\.LFO\.5m/B9.Cockpit.MK2.Body.5m/g" \
        -e "s/B9\.Aero\.HL\.Extension\.B2\.LF/B9.Aero.HL.Extension.B1/g" \
        -e "s/B9\.Aero\.HL\.Extension\.B3\.LFO/B9.Aero.HL.Extension.B1/g" \
        -e "s/B9\.Aero\.HL\.Extension\.B4\.RCS/B9.Aero.HL.Extension.B1/g" \
        -i$BAKDATE "$1"
    echo ". done"
}

echo "Looking for craft files in $PWD"
for f in *.craft
do
    deprecate "$f"
done

echo "Looking for persistence files in $PWD"
for f in *.sfs
do
    deprecate "$f"
done

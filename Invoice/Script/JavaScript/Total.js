function GetIndexValue(txt) {
    const number = _getInvoiceTotal(txt);
    return number;
}

function _getInvoiceTotal(txt) {
    const number = txt.match(
        /(?<=(GRAND TOTAL|Balance Due|TOTAL DUE|AMOUNT DUE)([:])?(\t+|\s+))(.*)(\d*)?.?(\d*)?(\d+)/g
    );
    if (number === null) {
        var lines = txt.split("\n");
        for (var i = 0; i < lines.length; i++) {
            var line_num = lines[i].match(
                /\bGRAND TOTAL|\bBalance Due|\bTOTAL DUE|\b Total Due |\bAMOUNT DUE|\bAmount Due/g
            );
            if (line_num !== null) {
                num = i;
                var line_content = line_num[0];
            }
        }
        if (num !== 0) {
            res = lines[num + 1].replace(":", ".");
        } else {
            res = "Unknown Total";
        }
    } else {
        res = number[0].replace(":", ".");
    }
    return res;
}

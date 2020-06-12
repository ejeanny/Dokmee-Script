function GetIndexValue(txt) {
    const number = _getInvoiceNumber(txt);
    return number;
}

function _getInvoiceNumber(txt) {
    const number = txt.match(
        /(?<=(INVOICE#|Order Number|Invoice Number|Invoice No.|INVOICE NO:|Purchase Order|Receipt #|NO.|INVOICE #)([:])?(\t+|\s+))(.*)(\d*)?.?(\d*)?(\d+)/g
    );
    if (number === null) {
        var lines = txt.split("\n");
        for (var i = 0; i < lines.length; i++) {
            var line_num = lines[i].match(
                /\bINVOICE NO.|\bInvoice Reference|\binvoice Reference/g
            );
            if (line_num !== null) {
                num = i;
                var line_content = line_num[0];
            }
        }
        if (num !== 0) {
            format = _getFormat(line_content);
            var to_clean = cleanLine(lines[num + 1]);
            res = to_clean;
        } else {
            res = "Unknown number";
        }
    } else {
        res = number[0].replace(/[^0-9,]/g, "");
    }
    return res;
}
function cleanLine(line) {
    var clean_line = "";
    switch (format) {
        case 0:
            line = line.replace(
                /\b[0-9]{1,2}(-)[0-9]{1,2}(-)[0-9]{2,4}|\b[0-9]{1,2}(\/)[0-9]{1,2}(\/)[0-9]{2,4}/g,
                ""
            );
            line = line.replace(/[^0-9,]/g, "");
            break;
        case 1:
            var found = line.match(/\b\((.*?)\)/g);
            clean_line = found[0].replace(/[^0-9,]/g, "");
            break;
        default:
            clean_line = line;
            break;
    }
    return clean_line;
}

function _getFormat(value) {
    switch (value) {
        case "INVOICE NO.":
            format = 0;
            break;
        case "Invoice Reference":
        case "invoice Reference":
            format = 1;
            break;
        default:
            format = null;
            break;
    }
    return format;
}

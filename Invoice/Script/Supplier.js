function GetIndexValue(txt) {
    const supplier = _getInvoiceSupplier(txt);
    return supplier;
}

function _getInvoiceSupplier(txt) {
    const supplierName = txt.match(
        /(?<=(Account Name|ACCT NAME|ACCOUNT NAME|Acct Name|ACCT Name)([:])?(\t+|\s+))(.*)(\d*)?.?(\d*)?/g
    );
    if (supplierName === null) {
        var lines = txt.split("\n");
        for (var i = 0; i < lines.length; i++) {
            var line_num = lines[i].match(/\bLLC.|\bLLC|\bCo.|\bInc.|\bINC./g);
            if (line_num) {
                return lines[i];
            }
        }

        res = "Unknown Supplier";
    } else {
        res = supplierName[0];
    }
    return res;
}

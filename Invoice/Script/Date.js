const months = [
    "january",
    "february",
    "march",
    "april",
    "may",
    "june",
    "july",
    "august",
    "september",
    "october",
    "november",
    "december",
];

function GetIndexValue(txt) {
    const date = _getDocumentDate(txt);
    return date;
}

function _getDocumentDate(txt) {
    let date;
    let res;
    date = txt.match(
        /\b[0-9]{1,2}(-)[0-9]{1,2}(-)[0-9]{2,4}|\b[0-9]{1,2}(\/)[0-9]{1,2}(\/)[0-9]{2,4}|\b[0-9]{1,2}(~)[0-9]{1,2}(-)[0-9]{2,4}|\b[0-9]{1,2}(\^)[0-9]{1,2}(-)[0-9]{2,4}/g
    );
    if (date) {
        res = date.length > 0 ? date[0] : "";
    } else {
        date = txt.match(
            /(?<=(Date|DATE|Issued on|ISSUED ON)([:|;|\s])?(\t+|\s+))(.*)(\d*)?.?(\d*)?(\d+)/g
        );
        if (!date) {
            //TODO Date algorithm
        }
        res = _refactorDate(date[0]);
    }

    return res;
}

function _refactorDate(stringDate) {
    const date = new Date(stringDate);
    let month = date.getMonth() + 1;
    if (month < 10) {
        month = "0" + month;
    }
    const day = date.getDate();
    const year = date.getFullYear();
    const fullDate = day + "/" + month + "/" + year;
    return fullDate;
}

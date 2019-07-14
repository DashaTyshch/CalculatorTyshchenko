$(document).ready(function () {

    var calc = $('.calculator');
    var calcDisplay = calc.find('.calculator__result');
    var calcButton = calc.find('.calculator__button');
    var calcClear = calc.find('.calculator__clear');
    var calcEqual = calc.find('.calculator__equal');
    var calcBackSpace = calc.find('.calculator__backspace');

    calcButton.on('click', function () {
        calcDisplay.val(calcDisplay.val() + $(this).text());
    });

    calcBackSpace.on('click', function () {
        calcDisplay.val(calcDisplay.val().substring(0, calcDisplay.val().length - 1));
    });

    calcClear.on('click', function () {
        calcDisplay.val('');
    });

    calcEqual.on('click', function () {
        const expr = encodeURIComponent(calcDisplay.val());
        $.getJSON(`Home/Calculate?expression=${expr}`, function (data, status) {
            var res = data.Result;
            if (res != null)
                calcDisplay.val(res);
            else {
                calcDisplay.val("");
                alert("Invalid input!");
            }
        });
    });
});
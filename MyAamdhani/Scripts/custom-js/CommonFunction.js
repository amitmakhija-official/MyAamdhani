var commonFunctions = {
    isNumberOnly: function (event) {
        event = (event) ? event : window.event;
        var charCode = (event.which) ? event.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    },
    PageLoader: function () {
        MakeAndCallLoader();
    },
    SessionTimeOut: (function (options) {
        var _getModal, _timeLeft, _popupTimer, _countDownTimer, _popUpModal, _timerSpan;
        _getModal = CreateSessionExpireWarningModal();
        _timerSpan = $(_getModal.find('#CountDownHolder'));
        _popUpModal = $(_getModal);
        var defaults = {
            timer: 20 * 60 * 1000,
            countDowntimer: 20,
            returnUrl: '',
            aliveUrl: ''
        };
        var settings = $.extend({}, defaults, options);
        var timerPlay = {
            stopTimers: function () {
                clearTimeout(_popupTimer);
                clearTimeout(_countDownTimer);
            },
            updateCountDown: function () {
                var seconds = _timeLeft;
                if (seconds < 10)
                    seconds = "0" + seconds;
                _timerSpan.text(seconds + ' seconds');
                if (_timeLeft > 0) {
                    _timeLeft--;
                    _countDownTimer = setTimeout(timerPlay.updateCountDown, 1000);
                } else {
                    timerPlay.logOut();
                }
            },
            showPopUp: function () {
                $(_popUpModal).modal('show');
                _timeLeft = settings.countDowntimer;
                timerPlay.updateCountDown();
            },
            schedulePopup: function () {
                timerPlay.hidePopup();
                timerPlay.stopTimers();
                _popupTimer = setTimeout(timerPlay.showPopUp, settings.timer);
            },
            hidePopup: function () {
                $(_popUpModal).modal('hide');
            },
            sendKeepAlive: function () {
                timerPlay.stopTimers();
                timerPlay.hidePopup();
                $.ajax({
                    type: "GET",
                    url: settings.aliveUrl,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function successFunc(response) {
                        settings.timer = response.delayTime;
                        timerPlay.schedulePopup();
                    },
                    error: function () {
                    }
                });
            },
            logOut: function () {
                var formLength = $('form#publicAutoVehicleForm').length || $('form#commAutoVehicleForm').length;
                if (formLength !== undefined && formLength > 0) {
                    timerPlay.hidePopup();
                    $(document).find('#savingItemLink_5').trigger('click');
                }
                document.location = settings.returnUrl;
            }
        }
        _getModal.find('#btnKeepAlive').on('click', timerPlay.sendKeepAlive)
        _getModal.find('#btnSessionLogout').on('click', timerPlay.hidePopup)
        return timerPlay;
    }),
    blockPage: function () {
        var loaderDiv = $(document).find('#loadingParent');
        if (!loaderDiv || loaderDiv.length === 0) {
            commonFunctions.PageLoader();
            loaderDiv = $(document).find('#loadingParent');
        }
        return {
            showLoader: function () {
                loaderDiv.show();
            },
            hideLoader: function () {
                loaderDiv.hide().fadeOut(100);
            },
        };
    },
    SearchInArray: function (nameKey, value, myArray) {
        for (var i = 0; i < myArray.length; i++) {
            if (myArray[i].nameKey === nameKey) {
                return myArray[i];
            }
        }
    },
    SlideToggleSection: function (event) {
        $(this).parent().next().parent().find('.slide-section').slideToggle('fast');
        if ($(this).hasClass('glyphicon-collapse-down')) {
            $(this).removeClass('glyphicon-collapse-down').addClass('glyphicon-collapse-up');
        } else if ($(this).hasClass('glyphicon-collapse-up')) {
            $(this).removeClass('glyphicon-collapse-up').addClass('glyphicon-collapse-down');
        }
    },
    AddCommaThousand: function (event) {
        var item = $(event.target);
        var Num = item.val();
        Num += '';
        Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
        Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
        x = Num.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1))
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        return item.val(x1 + x2);

    },

    EmailRegx: function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    },
    ValidateEmail: function () {
        var email = $(this).val();
        if (commonFunctions.EmailRegx(email))
            $(this).removeClass('input-validation-error');
        else
            $(this).addClass('input-validation-error');
    },
    FirstUpperCase: function () {
        var item = $(this);
        var text = item.val();
        if (text != "" && text.length > 0)
            item.val(text.substr(0, 1).toUpperCase() + text.substr(1));
    },
    TwoDecimal: function (evt) {
        if ((evt.which != 46 || evt.target.value.indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
            //event it's fine

        }
        var input = evt.target.value;
        if ((input.indexOf('.') != -1) && (input.substring(input.indexOf('.')).length > 2)) {
            return false;
        }
    },
    _preventSpace: function () {
        if (event.which === 32)
            return false;
    },
    _defaultAddDecimal: (event, minDecimalPoints = 2, maxDecimalPoints = 2) => {
        var item = $(event.target);
        var itemVal = item.val().replace(/[,\s]/g, '');
        if (itemVal.length > 0) {
            itemVal = new Intl.NumberFormat('en-US', {
                minimumFractionDigits: !isNaN(minDecimalPoints) ? minDecimalPoints : minDecimalPoints,
                maximumFractionDigits: !isNaN(maxDecimalPoints) ? maxDecimalPoints : maxDecimalPoints,
            }).format(itemVal);

        }
        return item.val(itemVal);
    }
}
function MakeAndCallLoader() {
    var elementStyles = {
        loadingParent: {
            'display': 'none',
            'position': 'fixed',
            'background': '#000',
            'width': '100%',
            'height': '100%',
            'top': 0,
            'bottom': 0,
            'right': 0,
            'left': 0,
            'z-index': 9999,
            'opacity': 0.9,
            'padding': 0,
        },
        loadingImgParent: {
            //'width': '150px',
            'position': 'absolute',
            'top': '50%',
            'left': '50%',
            'transform': 'translate(-50%, -50%)',
            'padding': '2em',
            'padding': 0
        },
        loadingImg: {
            'width': '200px',
            'border-radius': '10px'
        },
        loadingCloseButton: {
            'font-size': '30px',
            'background': 'rgba(198, 159, 18, 0.34)',
            'position': 'fixed',
            'right': '0',
            'top': '0',
            'border-radius': '0 10px',
            'cursor': 'pointer',
            'width': '35px',
            'height': '35px',
            'text-align': 'center',
            'line-height': '35px',
            'color': 'red',
            'display': 'none'
        }
    };
    var loaderParent = $('<div/>').prop({ id: 'loadingParent' }).css(elementStyles.loadingParent);
    var loaderImageParent = $('<div/>').prop({ id: 'loadingImgParent' }).css(elementStyles.loadingImgParent);
    var loadingImg = $('<img/>').prop({ 'src': '/LoaderImage/Ellipsis.gif' }).css(elementStyles.loadingImg);
    var loaderCloseIcon = $('<span/>').prop({ id: 'loaderCloseIcon' }).html('&times;').css(elementStyles.loadingCloseButton);
    loaderParent = loaderParent.append(loaderImageParent.append(loadingImg, loaderCloseIcon));
    $(document.body).append(loaderParent);
}
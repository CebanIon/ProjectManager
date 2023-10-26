'use strict';

const projectManager = {
    ExecutionResult: {
        OK: 1,
        KO: 2,
        ERROR: 3,
        NOTVALID: 4,
        EXCEPTION: 5
    },
    BuildActionUrl: function (controller, action, id) {
        var _url = getApplicationRoot();

        if (typeof controller === 'string' && controller.length > 0) {
            _url += '/' + controller
        }

        if (typeof action === 'string' && action.length > 0) {
            _url += '/' + action
        }

        if (typeof id === 'string' || typeof id === 'number') {
            _url += '/' + id;
        }
        return _url;
    },
    Ajax: function (url, data, type, additionalParams) {
        let params = $.extend({ url: url, type: type, data: data }, additionalParams);
        return $.ajax(params);
    },
    ShowPartialView: function (selector, parameters, $callback) {
        let url = this.BuildActionUrl(parameters.controller, parameters.action, parameters.id);
        this.Ajax(url).done(function (response) {
            if (response != null) {
                $(selector).html(null);
                $(selector).html(response);
            }
        });
    },
    ToastResult: function (response) {
        switch (response.result) {
            case ExecutionResult.OK:
                return 'images/success.png';
            case ExecutionResult.ERROR:
                return 'images/error.png';
            case ExecutionResult.INFO:
                return 'images/info.png';
        }
    },
    ShowToast: function (selector, response) {
        if (response.title !== null && response.message !== null) {
            $('#toastIcon').src = ToastResult(response);
            $('#toastTitle').html(response.title);
            $('$toastMessage').html(response.message);

            $('#toast').html('<partial name="~/Views/Shared/Components/_Toast.cshtml" />');
        }
    },
}
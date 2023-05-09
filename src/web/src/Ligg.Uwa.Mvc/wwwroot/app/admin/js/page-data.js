; (function (window) {
    "use strict";
    var pageData = {};

    function initPageData(pageId) {
        $.ajax({
            url: ctx + 'Sys/Config/GetPageConfigDetailsJsonToFrontEnd?pageId=' + pageId,
            type: "get",
            async: false,
            success: function (rst) {
                if (rst.Flag == 1) {
                    pageData = rst.Data;
                }
                else {
                    ys.msgError(rst.Message);
                }
            }
        });
    }

    function initPageButtons(masterId) {
        var removableBtnArr = getConfigDetailsByMasterId(masterId);
        if (removableBtnArr) {
            if (removableBtnArr.length > 0) {
                var btnArr = [];
                $('#toolbar').find('a').each(function (i, elmt) {
                    btnArr.push(elmt.id);
                });
                $.each(btnArr, function (index, val) {
                    for (var j = 0; j < removableBtnArr.length; j++) {
                        if (removableBtnArr[j].Attribute1.toLowerCase() == val.toLowerCase())
                            $("#" + val).remove();
                    }
                });
            }
        }


    }

    function getConfigDetailsByMasterId(masterId) {
        var arr = [];
        if (pageData) {
            $(pageData).each(function (i, val) {
                if (val.MasterId != null) {
                    if (val.MasterId == masterId) {
                        arr.push(val);
                    }
                }
            });
        }
        return arr;
    }
    function getConfigDetailNameByIdWithStyle(id) {
        var rst = '';
        if (pageData) {
            $(pageData).each(function (i, val) {
                if (val.Id == id) {
                    if (val.Style) {
                        rst = '<span class="badge badge-' + val.Style + '">' + val.Name + '</span>';
                        return false;
                    }
                    else {
                        rst = val.Name;
                        return false;
                    }
                }
            });
        }

        return rst;
    }

    function getConfigDetailDefaultIdByMasterId(masterId) {
        var rst = 0;
        var arr = getConfigDetailsByMasterId(masterId);
        if (arr) {
            $(arr).each(function (i, val) {
                if (val.IsDefault) {
                    rst = val.Id;
                    return rst;
                }
                i++;
            });
        }
        return rst;

    }

    function checkPortal() {
        var msg = '?addlMsg=pls open page under porta';
        if (self != top) {
            if (typeof (top.portal) == "undefined") window.location.replace('/home/error/wurl' + msg);
            else if (!top.portal) window.location.replace('/home/error/wurl' + msg);
        }
        else {
            if (typeof (portal) == "undefined") window.location.replace('/home/error/wurl' + 'l');
            else if (!portal) window.location.replace('/home/error/wurl' + msg);
        }
    }

    function GetUrlRelativePath() {
        var url = document.location.toString();
        var urlArr = url.split("//");

        var start = urlArr[1].indexOf("/");
        var relUrl = urlArr[1].substring(start + 1);

        if (relUrl.indexOf("?") != -1) {
            relUrl = relUrl.split("?")[0];
        }
        return relUrl;
    }

    //initPageData();
    checkPortal();
    window.initPageData = initPageData;
    window.initPageButtons = initPageButtons;
    window.getConfigDetailsByMasterId = getConfigDetailsByMasterId;
    window.getConfigDetailDefaultIdByMasterId = getConfigDetailDefaultIdByMasterId;
    window.getConfigDetailNameByIdWithStyle = getConfigDetailNameByIdWithStyle;
})(window);
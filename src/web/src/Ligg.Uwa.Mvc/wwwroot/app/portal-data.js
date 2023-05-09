; (function (window) {
    "use strict";
    var portal = {};
    var portalData = {};

    function setPortal(portalObj) {
        portal= portalObj;
    }
    function getPortal() {
        return portal;
    }

    function initPortalData() {
        ys.ajax({
            url: ctx + 'Sys/Config/GetPortalConfigDetailsJsonToFrontEnd?id=' + portal.Id,
            type: "get",
            async: false,
            success: function (rst) {
                if (rst.Flag == 1) {
                    portalData = rst.Data;
                } else {
                    ys.msgError(rst.Message);
                }
                

            }
        });
    }



    /*initPortalData();*/
    window.setPortal = setPortal;
    window.getPortalObj = getPortal;
    window.portal = portal;
    window.initPortalData = initPortalData;

})(window);
var x =$CS.getText("WorkOrder_Fields_UDF_CHAR37")
switch(x) {
    case "test1":
    $CS.removeAllOptions(["WorkOrder_Fields_UDF_CHAR39"])
                $CS.addOptions("WorkOrder_Fields_UDF_CHAR39", ["test11"])
    break
    case "test2":
                $CS.removeAllOptions(["WorkOrder_Fields_UDF_CHAR39"])
                $CS.addOptions("WorkOrder_Fields_UDF_CHAR39", ["test22"])
    break
    case "test3":
                $CS.removeAllOptions(["WorkOrder_Fields_UDF_CHAR39"])
                $CS.addOptions("WorkOrder_Fields_UDF_CHAR39", ["test33"])
    break
    case "test4":
                $CS.removeAllOptions(["WorkOrder_Fields_UDF_CHAR39"])
                $CS.addOptions("WorkOrder_Fields_UDF_CHAR39", ["test44"])
    break
        case "test5":
                $CS.removeAllOptions(["WorkOrder_Fields_UDF_CHAR39"])
                $CS.addOptions("WorkOrder_Fields_UDF_CHAR39", ["test55"])
        default:
                $CS.removeAllOptions(["WorkOrder_Fields_UDF_CHAR39"])
    break
}

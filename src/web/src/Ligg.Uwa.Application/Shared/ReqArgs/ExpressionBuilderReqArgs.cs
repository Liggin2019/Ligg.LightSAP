
using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Operations;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using System;

namespace Ligg.Uwa.Application.Shared
{
    public class ExpressionBuilderReqArgs
    {
        public string Text { get; set; }
        public string Mark { get; set; }

        public int? JudgementOption { get; set; }
        public int? Judgement1Option { get; set; }
        public bool? Judgement { get; set; }
        public bool? Judgement1 { get; set; }

        public int? OptionInteger { get; set; }
        public int? OptionInteger1 { get; set; }

        public string OptionString { get; set; }
        public string OptionString1 { get; set; }


        public float? StartNumeral { get; set; }
        public float? EndNumeral { get; set; }
        public float? StartNumeral1 { get; set; }
        public float? EndNumeral1 { get; set; }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public DateTime? StartDateTime1 { get; set; }
        public DateTime? EndDateTime1 { get; set; }

        public string Formular { get; set; }


    }

    internal static class ExpressionBuilderEx
    {
        internal static void GetAndExpression(this ExpressionBuilderReqArgs reqArgs, IFilter filter)
        {
            //reqArgs.Judgement = null;
            if (reqArgs.JudgementOption != null & reqArgs.JudgementOption != -1)
            {
                if (reqArgs.JudgementOption == 1)
                    reqArgs.Judgement = true;
                else if (reqArgs.JudgementOption == 0)
                    reqArgs.Judgement = false;
            }

            if (reqArgs.Judgement1Option != null & reqArgs.Judgement1Option != -1)
            {
                if (reqArgs.Judgement1Option == 1)
                    reqArgs.Judgement1 = true;
                else if (reqArgs.Judgement1Option == 0)
                    reqArgs.Judgement1 = false;
            }


            GetExpression(reqArgs, filter);
            if (!reqArgs.Text.IsNullOrEmpty() & !GlobalContext.SystemSetting.SupportMultiLanguages)
            {
                filter.StartGroup();
                GetTextExpression(reqArgs, filter, false);
            }

            if (!reqArgs.Mark.IsNullOrEmpty())
            {
                filter.StartGroup();
                GetTextExpression(reqArgs, filter, true);
            }
        }
        internal static void GetTextExpression(this ExpressionBuilderReqArgs reqArgs, IFilter filter, bool isMark)
        {
            var reqArgsText = reqArgs.Text ?? "".Trim();
            var dict = reqArgs.Formular.ConvertLdictToDictionary(true, true,true);
            var tmpArr = reqArgs.Formular.ExtractSubStringsByTwoDifferentItentifiers("Text:", ";", true);
            var dictValue = dict.GetLdictValue("Text");
            if (isMark)
            {
                reqArgsText = reqArgs.Mark ?? "".Trim();
                //tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("MlText:", ";", true);
                dictValue = dict.GetLdictValue("Mark");
            }
            if (!reqArgsText.IsNullOrEmpty())
            {
                {
                    //var textFieldArr = tmpArr[0].SplitByChar(',', true, true);
                    var textFieldArr = dictValue.GetLarrayArray(true, true);
                    if (textFieldArr != null)
                        if (textFieldArr.Length > 0)
                        {
                            var len = textFieldArr.Length;
                            var i = 0;
                            foreach (var textField in textFieldArr)
                            {
                                if (reqArgsText.StartsWith("="))
                                {
                                    var val = reqArgsText;
                                    val = val.Substring(1, val.Length - 1);
                                    if (i == len - 1)
                                    {
                                        filter.By(textField, Operation.EqualTo, val);
                                    }
                                    else filter.By(textField, Operation.EqualTo, val, Connector.Or);
                                }
                                else if (reqArgsText.StartsWith("*"))
                                {
                                    var val = reqArgsText.Trim();
                                    val = val.Substring(1, val.Length - 1);
                                    if (i == len - 1)
                                    {
                                        filter.By(textField, Operation.EndsWith, val);
                                    }
                                    else filter.By(textField, Operation.EndsWith, val, Connector.Or);
                                }
                                else if (reqArgsText.EndsWith("*"))
                                {
                                    var val = reqArgsText;
                                    val = val.Substring(0, val.Length - 1);
                                    if (i == len - 1)
                                    {
                                        filter.By(textField, Operation.StartsWith, val);
                                    }
                                    else filter.By(textField, Operation.StartsWith, val, Connector.Or);
                                }
                                else
                                {
                                    if (i == len - 1)
                                    {
                                        filter.By(textField, Operation.Contains, reqArgsText);
                                    }
                                    else filter.By(textField, Operation.Contains, reqArgsText, Connector.Or);
                                }

                                i++;
                            }
                        }
                }
            }
        }

        internal static void GetExpression(this ExpressionBuilderReqArgs reqArgs, IFilter filter)
        {
            var dict = reqArgs.Formular.ConvertLdictToDictionary(true, true,true);
            if (reqArgs.Judgement != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("Judgement:", ";", true);
                //if (tmpArr != null)
                {
                    //var field = tmpArr[0].Trim();
                    var field = dict.GetLdictValue("Judgement");
                    if (!field.IsNullOrEmpty())
                    {
                        filter.By(field, Operation.EqualTo, reqArgs.Judgement, Connector.And);
                    }
                }
            }

            if (reqArgs.Judgement1 != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("Judgement1:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("Judgement1");
                    if (!field.IsNullOrEmpty())
                    {
                        filter.By(field, Operation.EqualTo, reqArgs.Judgement1, Connector.And);
                    }
                }
            }

            if (reqArgs.OptionInteger != -1 & reqArgs.OptionInteger != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("OptionInteger:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("OptionInteger");
                    if (!field.IsNullOrEmpty())
                    {
                        filter.By(field, Operation.EqualTo, reqArgs.OptionInteger, Connector.And);
                    }
                }
            }

            if (reqArgs.OptionInteger1 != -1 & reqArgs.OptionInteger1 != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("OptionInt1:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("OptionInteger1");
                    if (!field.IsNullOrEmpty())
                    {
                        filter.By(field, Operation.EqualTo, reqArgs.OptionInteger1, Connector.And);
                    }
                }
            }

            if (!reqArgs.OptionString.IsNullOrEmpty())
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("OptionString:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("OptionString");
                    if (!field.IsNullOrEmpty())
                    {
                        filter.By(field, Operation.EqualTo, reqArgs.OptionString, Connector.And);
                    }
                }
            }
            if (!reqArgs.OptionString1.IsNullOrEmpty())
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("OptionString1:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("OptionString1");
                    if (!field.IsNullOrEmpty())
                    {
                        filter.By(field, Operation.EqualTo, reqArgs.OptionString1, Connector.And);
                    }
                }
            }

            if (reqArgs.StartDateTime != null | reqArgs.EndDateTime != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("Time:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("DateTime");
                    if (!field.IsNullOrEmpty())
                    {
                        var startVal = reqArgs.StartDateTime ?? DateTime.MinValue;
                        var endVal = reqArgs.EndDateTime ?? DateTime.MaxValue;

                        filter.By(field, Operation.Between, startVal, endVal, Connector.And);
                    }
                }
            }

            if (reqArgs.StartDateTime1 != null | reqArgs.EndDateTime1 != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("Time1:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("DateTime1");
                    if (!field.IsNullOrEmpty())
                    {
                        var startVal = reqArgs.StartDateTime1 ?? DateTime.MinValue;
                        var endVal = reqArgs.EndDateTime1 ?? DateTime.MaxValue;

                        filter.By(field, Operation.Between, startVal, endVal, Connector.And);
                    }
                }
            }

            if (reqArgs.StartNumeral != null | reqArgs.EndNumeral != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("Long:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("Numeral");
                    if (!field.IsNullOrEmpty())
                    {
                        var startVal = reqArgs.StartNumeral ?? long.MinValue;
                        var endVal = reqArgs.EndNumeral ?? long.MaxValue;
                        //filter.By(fieldName, Operation.Between, 555, 666);//error; 555 is int; 
                        //filter.By(fieldName, Operation.Between, (long)555, (long)666);//ok; 
                        filter.By(field, Operation.Between, startVal, endVal, Connector.And);
                    }
                }
            }

            if (reqArgs.StartNumeral1 != null | reqArgs.EndNumeral1 != null)
            {
                //var tmpArr = reqArgs.Formular.SplitByTwoDifferentStrings("Long1:", ";", true);
                //if (tmpArr != null)
                {
                    var field = dict.GetLdictValue("Numeral1");
                    if (!field.IsNullOrEmpty())
                    {
                        var startVal = reqArgs.StartNumeral1 ?? long.MinValue;
                        var endVal = reqArgs.EndNumeral1 ?? long.MaxValue;
                        filter.By(field, Operation.Between, startVal, endVal, Connector.And);
                    }
                }
            }


        }


    }

}

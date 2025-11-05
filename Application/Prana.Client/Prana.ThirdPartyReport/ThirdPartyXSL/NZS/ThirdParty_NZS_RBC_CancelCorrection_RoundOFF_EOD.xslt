<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public int RoundOff(double Qty)
    {

    return (int)Math.Round(Qty,0);
    }
  </msxsl:script>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty!= 'Transfer' or CounterParty!= 'Corporate action' or CounterParty!= 'Cost Adjustment']">

        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test ="TaxLotState != 'Amemded'">

            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select="'false'"/>
              </RowHeader>

              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <xsl:variable name="varTaxlotState">
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="'CREATE'"/>
                  </xsl:when>

                  <xsl:when test="TaxLotState='Deleted'">
                    <xsl:value-of select ="'DELETE'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <ACTION>
                <xsl:value-of select="$varTaxlotState"/>
              </ACTION>

              <EMETTEUR>
                <xsl:value-of select="'UKJUPITER'"/>
              </EMETTEUR>



              <REF-EXT>
                <xsl:value-of select="PBUniqueID"/>
              </REF-EXT>

              <INTERNAL_ORIGID>
                <xsl:value-of select="''"/>
              </INTERNAL_ORIGID>

              <INTERNAL_ID>
                <xsl:value-of select="''"/>
              </INTERNAL_ID>

              <INTERNAL_STATUS>
                <xsl:value-of select="''"/>
              </INTERNAL_STATUS>

              <EXTERNAL_ORIGID>
                <xsl:value-of select="''"/>
              </EXTERNAL_ORIGID>

              <EXTERNAL_ID>
                <xsl:value-of select="''"/>
              </EXTERNAL_ID>

              <EXTERNAL_STATUS>
                <xsl:value-of select="''"/>
              </EXTERNAL_STATUS>

              <DATE_OUT>
                <xsl:value-of select="''"/>
              </DATE_OUT>

              <TIME_OUT>
                <xsl:value-of select="''"/>
              </TIME_OUT>

              <ERROR_MESSAGE>
                <xsl:value-of select="''"/>
              </ERROR_MESSAGE>

              <xsl:variable name="varSide1">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'SEL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <OPE_TYP>
                <xsl:value-of select="$varSide1"/>
              </OPE_TYP>

              <MGP>
                <xsl:value-of select="'30355'"/>
              </MGP>

              <CASH-COR>
                <xsl:value-of select="''"/>
              </CASH-COR>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'RBC'"/>
              </xsl:variable>
              <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
              <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CurrencySymbol"/>


              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_Name]/@BICCode"/>
              </xsl:variable>

              <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerName">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerName"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
              </xsl:variable>
              <BRK-REF>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC != ''">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYUS33'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BRK-REF>

              <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
              </xsl:variable>

              <xsl:variable name="Broker">
                <xsl:choose>

                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <BRK-NAM>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerName!=''">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerName"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BRK-NAM>

              <BRK-ACC>
                <xsl:value-of select="''"/>
              </BRK-ACC>
              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
              </xsl:variable>

              <SUB-CLR-ACC>
                <xsl:choose>

                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'0161'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-CLR-ACC>



              <SUB-CLR-COD>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-CLR-COD>



              <SUB-REF>
                <xsl:choose>

                  <xsl:when test="Exchange ='FRA'">
                    <xsl:value-of select="'PARBDEFFXXX'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType = 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-REF>

              <SUB-DES>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerName!=''">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerName"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-DES>

              <NIV-CPT1>
                <xsl:value-of select="''"/>
              </NIV-CPT1>

              <xsl:variable name="varCLRACC">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'0161'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <CLR-ACC>
                <xsl:choose>

                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'0161'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CLR-ACC>

              <xsl:variable name="varCLRCod">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <CLR-COD>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CLR-COD>

              <NIV-BIC3>
                <xsl:value-of select="''"/>
              </NIV-BIC3>

              <NIV-LIB3>
                <xsl:value-of select="''"/>
              </NIV-LIB3>

              <NIV-CPT3>
                <xsl:value-of select="''"/>
              </NIV-CPT3>

              <NIV-CLR-ACC3>
                <xsl:value-of select="''"/>
              </NIV-CLR-ACC3>

              <NIV-CLR-COD3>
                <xsl:value-of select="''"/>
              </NIV-CLR-COD3>

              <NIV-BIC4>
                <xsl:value-of select="''"/>
              </NIV-BIC4>

              <NIV-LIB4>
                <xsl:value-of select="''"/>
              </NIV-LIB4>

              <NIV-CPT4>
                <xsl:value-of select="''"/>
              </NIV-CPT4>

              <NIV-CLR-ACC4>
                <xsl:value-of select="''"/>
              </NIV-CLR-ACC4>

              <NIV-CLR-COD4>
                <xsl:value-of select="''"/>
              </NIV-CLR-COD4>
              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <TRA-DAT>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TRA-DAT>



              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SET-DAT>
                <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>

              </SET-DAT>

              <TRS-CUR>
                <xsl:value-of select="CurrencySymbol"/>
              </TRS-CUR>

              <xsl:variable name="varSecurityIDType">
                <xsl:choose>
                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'IC'"/>
                  </xsl:when>


                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="'GB'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="'US'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SEC-TYP>
                <xsl:value-of select="$varSecurityIDType"/>
              </SEC-TYP>

              <xsl:variable name="varSecurityID">
                <xsl:choose>
                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="ISIN"/>
                  </xsl:when>


                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="CUSIP"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SEC-COD>
                <xsl:value-of select="$varSecurityID"/>
              </SEC-COD>

              <SEC-DES>
                <xsl:value-of select="CompanyName"/>
              </SEC-DES>


              <QTY>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff(OrderQty)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number(OrderQty,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </QTY>

              <PRI>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff(AvgPrice)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number(AvgPrice,'#.######')"/>
                  </xsl:otherwise>
                </xsl:choose>

              </PRI>

              <INT-MOD>
                <xsl:value-of select="''"/>
              </INT-MOD>

              <FEE-CUR>
                <xsl:value-of select="CurrencySymbol"/>
              </FEE-CUR>

              <STK-EXC>
                <xsl:value-of select="''"/>
              </STK-EXC>

              <SET-CUR>
                <xsl:value-of select="CurrencySymbol"/>
              </SET-CUR>

              <CHG-RAT>
                <xsl:value-of select="''"/>
              </CHG-RAT>
              <xsl:variable name="varCommission">
                <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </xsl:variable>
              <BRK-FEE>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff($varCommission)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varCommission,'0.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BRK-FEE>

              <TAX-FEE>
                <xsl:value-of select="''"/>
              </TAX-FEE>

              <OTH-FEE>
                <xsl:value-of select="format-number((OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingBrokerFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + StampDuty),'0.##')"/>
                <!-- <xsl:value-of select="SecFee + OtherBrokerFees + ClearingBrokerFee + MiscFees + OccFee + OrfFee + StampDuty + TransactionLevy"/> -->
              </OTH-FEE>

              <INT-AMT>
                <xsl:value-of select="''"/>
              </INT-AMT>

              <INT-TAX>
                <xsl:value-of select="''"/>
              </INT-TAX>

              <INT-DAY>
                <xsl:value-of select="''"/>
              </INT-DAY>


              <TRS-GRO-AMT>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff(AvgPrice * OrderQty)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number(AvgPrice * OrderQty,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TRS-GRO-AMT>

              <TRS-NET-AMT>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff($varNetamount)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varNetamount,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TRS-NET-AMT>

              <xsl:variable name="varFXRate">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol != SettlCurrency">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <xsl:variable name="varNetAmountBase">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol = SettlCurrency">
                    <xsl:value-of select="NetAmount"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="NetAmount * $varFXRate"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SET-NET-AMT>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff($varNetamount)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varNetamount,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SET-NET-AMT>


              <FEE-AMT>
                <xsl:value-of select="''"/>
              </FEE-AMT>

              <FM-TXT>
                <xsl:value-of select="''"/>
              </FM-TXT>

              <COM-TXT>
                <xsl:value-of select="''"/>
              </COM-TXT>

              <EVEN-TYP>
                <xsl:value-of select="'N'"/>
              </EVEN-TYP>

              <PSET-BIC>
                <xsl:choose>

                  <xsl:when test="Exchange ='FRA'">
                    <xsl:value-of select="'DAKVDEFFXXX'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PSET-BIC>

              <PSET-PAYS>
                <xsl:value-of select="''"/>
              </PSET-PAYS>

              <CUS-FEE>
                <xsl:value-of select="''"/>
              </CUS-FEE>

              <SCUS-FEE>
                <xsl:value-of select="''"/>
              </SCUS-FEE>

              <STAMP-FEE>
                <xsl:value-of select="''"/>
              </STAMP-FEE>

              <OPC-IO-TOT>
                <xsl:value-of select="''"/>
              </OPC-IO-TOT>

              <OPC-IO-REF>
                <xsl:value-of select="''"/>
              </OPC-IO-REF>

              <OPC-IO-ACQ>
                <xsl:value-of select="''"/>
              </OPC-IO-ACQ>

              <FACTOR>
                <xsl:value-of select="''"/>
              </FACTOR>

              <QTE-TYP>
                <xsl:value-of select="'UNIT'"/>
              </QTE-TYP>

              <VAT>
                <xsl:value-of select="''"/>
              </VAT>

              <TYP-TAU>
                <xsl:value-of select="''"/>
              </TYP-TAU>

              <SET-FLG>
                <xsl:value-of select="''"/>
              </SET-FLG>

              <DEL-COD>
                <xsl:value-of select="'A'"/>
              </DEL-COD>

              <DEP-FIN-REF>
                <xsl:value-of select="''"/>
              </DEP-FIN-REF>

              <DEP-FIN-DES>
                <xsl:value-of select="''"/>
              </DEP-FIN-DES>

              <DEP-FIN-ACC>
                <xsl:value-of select="''"/>
              </DEP-FIN-ACC>

              <PTG-CSH-ACC-TYP>
                <xsl:value-of select="''"/>
              </PTG-CSH-ACC-TYP>

              <PTG-CSH-ACC-NBR>
                <xsl:value-of select="''"/>
              </PTG-CSH-ACC-NBR>

              <SPREAD>
                <xsl:value-of select="''"/>
              </SPREAD>

              <MARKET-CLAIM-OPT-OUT>
                <xsl:value-of select="''"/>
              </MARKET-CLAIM-OPT-OUT>

              <EX-CUM-COUPON>
                <xsl:value-of select="''"/>
              </EX-CUM-COUPON>

              <PSAF-BIC>
                <xsl:value-of select="''"/>
              </PSAF-BIC>

              <INDIC1>
                <xsl:value-of select="'SETR//TRAD'"/>
              </INDIC1>

              <BLB-CODE>
                <xsl:value-of select="''"/>
              </BLB-CODE>

              <INDIC4>
                <xsl:value-of select="''"/>
              </INDIC4>

              <CUR-CHG>
                <xsl:value-of select="''"/>
              </CUR-CHG>

              <INDIC5>
                <xsl:value-of select="''"/>
              </INDIC5>

              <INDIC6>
                <xsl:value-of select="''"/>
              </INDIC6>

              <INDIC7>
                <xsl:value-of select="''"/>
              </INDIC7>

              <INDIC8>
                <xsl:value-of select="''"/>
              </INDIC8>

              <AMORQTY>
                <xsl:value-of select="''"/>
              </AMORQTY>

              <REPMATDAT>
                <xsl:value-of select="''"/>
              </REPMATDAT>

              <REPRATDAT>
                <xsl:value-of select="''"/>
              </REPRATDAT>

              <REPRATTYP>
                <xsl:value-of select="''"/>
              </REPRATTYP>

              <REPINTMIC>
                <xsl:value-of select="''"/>
              </REPINTMIC>

              <REPREVIND>
                <xsl:value-of select="''"/>
              </REPREVIND>

              <REPLEGIND>
                <xsl:value-of select="''"/>
              </REPLEGIND>

              <REPMDMIND>
                <xsl:value-of select="''"/>
              </REPMDMIND>

              <REPINTIND>
                <xsl:value-of select="''"/>
              </REPINTIND>

              <REPREFSEC>
                <xsl:value-of select="''"/>
              </REPREFSEC>

              <REPREFREP>
                <xsl:value-of select="''"/>
              </REPREFREP>

              <REPRATREP>
                <xsl:value-of select="''"/>
              </REPRATREP>

              <REPRATVAR>
                <xsl:value-of select="''"/>
              </REPRATVAR>

              <REPSPRRAT>
                <xsl:value-of select="''"/>
              </REPSPRRAT>

              <REPPRCRAT>
                <xsl:value-of select="''"/>
              </REPPRCRAT>

              <REPLOAMRG>
                <xsl:value-of select="''"/>
              </REPLOAMRG>

              <REPSECHRC>
                <xsl:value-of select="''"/>
              </REPSECHRC>

              <REPNBRTCD>
                <xsl:value-of select="''"/>
              </REPNBRTCD>

              <REPNBRNCO>
                <xsl:value-of select="''"/>
              </REPNBRNCO>

              <REPFRFAMT>
                <xsl:value-of select="''"/>
              </REPFRFAMT>

              <REPFRFCCY>
                <xsl:value-of select="''"/>
              </REPFRFCCY>

              <REPTERAMT>
                <xsl:value-of select="''"/>
              </REPTERAMT>

              <REPTERCCY>
                <xsl:value-of select="''"/>
              </REPTERCCY>

              <REPPRMAMT>
                <xsl:value-of select="''"/>
              </REPPRMAMT>

              <REPPRMCCY>
                <xsl:value-of select="''"/>
              </REPPRMCCY>

              <REPACRAMT>
                <xsl:value-of select="''"/>
              </REPACRAMT>

              <REPACRCCY>
                <xsl:value-of select="''"/>
              </REPACRCCY>

              <REPDEAAMT>
                <xsl:value-of select="''"/>
              </REPDEAAMT>

              <REPDEACCY>
                <xsl:value-of select="''"/>
              </REPDEACCY>

              <REPTPCAMT>
                <xsl:value-of select="''"/>
              </REPTPCAMT>

              <REPTPCCCY>
                <xsl:value-of select="''"/>
              </REPTPCCCY>

              <REPLEGNAR>
                <xsl:value-of select="''"/>
              </REPLEGNAR>

              <COMFIA>
                <xsl:value-of select="''"/>
              </COMFIA>

              <COMTRD>
                <xsl:value-of select="''"/>
              </COMTRD>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">

              <ThirdPartyFlatFileDetail>

                <RowHeader>
                  <xsl:value-of select="'false'"/>
                </RowHeader>

                <FileHeader>
                  <xsl:value-of select="'true'"/>
                </FileHeader>

                <TaxLotState>
                  <xsl:value-of select="'Deleted'"/>
                </TaxLotState>

                <xsl:variable name="varTaxlotState">
                  <xsl:choose>
                    <xsl:when test="TaxLotState='Allocated'">
                      <xsl:value-of select ="'CREATE'"/>
                    </xsl:when>

                    <xsl:when test="TaxLotState='Deleted'">
                      <xsl:value-of select ="'DELETE'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


                <ACTION>
                  <xsl:value-of select ="'DELETE'"/>
                </ACTION>



                <EMETTEUR>
                  <xsl:value-of select="'UKJUPITER'"/>
                </EMETTEUR>

                <REF-EXT>
                  <xsl:value-of select="PBUniqueID"/>
                </REF-EXT>

                <INTERNAL_ORIGID>
                  <xsl:value-of select="''"/>
                </INTERNAL_ORIGID>

                <INTERNAL_ID>
                  <xsl:value-of select="''"/>
                </INTERNAL_ID>

                <INTERNAL_STATUS>
                  <xsl:value-of select="''"/>
                </INTERNAL_STATUS>

                <EXTERNAL_ORIGID>
                  <xsl:value-of select="''"/>
                </EXTERNAL_ORIGID>

                <EXTERNAL_ID>
                  <xsl:value-of select="''"/>
                </EXTERNAL_ID>

                <EXTERNAL_STATUS>
                  <xsl:value-of select="''"/>
                </EXTERNAL_STATUS>

                <DATE_OUT>
                  <xsl:value-of select="''"/>
                </DATE_OUT>

                <TIME_OUT>
                  <xsl:value-of select="''"/>
                </TIME_OUT>

                <ERROR_MESSAGE>
                  <xsl:value-of select="''"/>
                </ERROR_MESSAGE>

                <xsl:variable name="varSide1">
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy'">
                      <xsl:value-of select="'BUY'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell'">
                      <xsl:value-of select="'SEL'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


                <OPE_TYP>
                  <xsl:value-of select="$varSide1"/>
                </OPE_TYP>

                <MGP>
                  <xsl:value-of select="'30355'"/>
                </MGP>

                <CASH-COR>
                  <xsl:value-of select="''"/>
                </CASH-COR>
                <xsl:variable name="PB_NAME">
                  <xsl:value-of select="'RBC'"/>
                </xsl:variable>
                <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
                <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CurrencySymbol"/>


                <xsl:variable name="THIRDPARTY_PSET_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_Name]/@BICCode"/>
                </xsl:variable>

                <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerName">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerName"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
                </xsl:variable>
                <BRK-REF>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC != ''">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
                    </xsl:when>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'DTCYUS33'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </BRK-REF>

                <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

                <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
                </xsl:variable>

                <xsl:variable name="Broker">
                  <xsl:choose>

                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>


                <BRK-NAM>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerName!=''">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerName"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </BRK-NAM>

                <BRK-ACC>
                  <xsl:value-of select="''"/>
                </BRK-ACC>
                <xsl:variable name = "PRANA_FUND_NAME">
                  <xsl:value-of select="AccountName"/>
                </xsl:variable>

                <xsl:variable name ="THIRDPARTY_FUND_CODE">
                  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
                </xsl:variable>

                <SUB-CLR-ACC>
                  <xsl:choose>

                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'0161'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SUB-CLR-ACC>



                <SUB-CLR-COD>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                    </xsl:when>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SUB-CLR-COD>



                <SUB-REF>
                  <xsl:choose>

                    <xsl:when test="Exchange ='FRA'">
                      <xsl:value-of select="'PARBDEFFXXX'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType = 'BIC'">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SUB-REF>

                <SUB-DES>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerName!=''">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerName"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SUB-DES>

                <NIV-CPT1>
                  <xsl:value-of select="''"/>
                </NIV-CPT1>

                <xsl:variable name="varCLRACC">
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'0161'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <CLR-ACC>
                  <xsl:choose>

                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'0161'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </CLR-ACC>

                <xsl:variable name="varCLRCod">
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <CLR-COD>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                      <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                    </xsl:when>
                    <xsl:when test="CurrencySymbol ='USD'">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </CLR-COD>

                <NIV-BIC3>
                  <xsl:value-of select="''"/>
                </NIV-BIC3>

                <NIV-LIB3>
                  <xsl:value-of select="''"/>
                </NIV-LIB3>

                <NIV-CPT3>
                  <xsl:value-of select="''"/>
                </NIV-CPT3>

                <NIV-CLR-ACC3>
                  <xsl:value-of select="''"/>
                </NIV-CLR-ACC3>

                <NIV-CLR-COD3>
                  <xsl:value-of select="''"/>
                </NIV-CLR-COD3>

                <NIV-BIC4>
                  <xsl:value-of select="''"/>
                </NIV-BIC4>

                <NIV-LIB4>
                  <xsl:value-of select="''"/>
                </NIV-LIB4>

                <NIV-CPT4>
                  <xsl:value-of select="''"/>
                </NIV-CPT4>

                <NIV-CLR-ACC4>
                  <xsl:value-of select="''"/>
                </NIV-CLR-ACC4>

                <NIV-CLR-COD4>
                  <xsl:value-of select="''"/>
                </NIV-CLR-COD4>
                <xsl:variable name="varTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="TradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>

                <TRA-DAT>
                  <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
                </TRA-DAT>



                <xsl:variable name="varSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="SettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SET-DAT>
                  <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>

                </SET-DAT>

                <TRS-CUR>
                  <xsl:value-of select="CurrencySymbol"/>
                </TRS-CUR>

                <xsl:variable name="varSecurityIDType">
                  <xsl:choose>
                    <xsl:when test="ISIN!=''">
                      <xsl:value-of select="'IC'"/>
                    </xsl:when>


                    <xsl:when test="SEDOL!=''">
                      <xsl:value-of select="'GB'"/>
                    </xsl:when>
                    <xsl:when test="CUSIP!=''">
                      <xsl:value-of select="'US'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <SEC-TYP>
                  <xsl:value-of select="$varSecurityIDType"/>
                </SEC-TYP>

                <xsl:variable name="varSecurityID">
                  <xsl:choose>
                    <xsl:when test="ISIN!=''">
                      <xsl:value-of select="ISIN"/>
                    </xsl:when>


                    <xsl:when test="SEDOL!=''">
                      <xsl:value-of select="SEDOL"/>
                    </xsl:when>
                    <xsl:when test="CUSIP!=''">
                      <xsl:value-of select="CUSIP"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <SEC-COD>
                  <xsl:value-of select="$varSecurityID"/>
                </SEC-COD>

                <SEC-DES>
                  <xsl:value-of select="CompanyName"/>
                </SEC-DES>

                <QTY>

                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                      <xsl:value-of select="my:RoundOff(OldExecutedQuantity)"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="format-number(OldExecutedQuantity,'#.##')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </QTY>

                <PRI>

                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                      <xsl:value-of select="my:RoundOff(OldAvgPrice)"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="format-number(OldAvgPrice,'#.######')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </PRI>

                <INT-MOD>
                  <xsl:value-of select="''"/>
                </INT-MOD>

                <FEE-CUR>
                  <xsl:value-of select="CurrencySymbol"/>
                </FEE-CUR>

                <STK-EXC>
                  <xsl:value-of select="''"/>
                </STK-EXC>

                <SET-CUR>
                  <xsl:value-of select="CurrencySymbol"/>
                </SET-CUR>

                <CHG-RAT>
                  <xsl:value-of select="''"/>
                </CHG-RAT>
                <xsl:variable name="varCommission1">
                  <xsl:value-of select="OldCommissionAmount + OldSoftCommissionAmount"/>
                </xsl:variable>
                <BRK-FEE>

                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                 
                      <xsl:value-of select="my:RoundOff($varCommission1)"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="format-number($varCommission1,'0.##')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </BRK-FEE>

                <TAX-FEE>
                  <xsl:value-of select="''"/>
                </TAX-FEE>

                <OTH-FEE>
                  <xsl:value-of select="format-number((OldOtherBrokerFees + OldClearingBrokerFee + OldTransactionLevy + OldClearingBrokerFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee + OldStampDuty),'0.##')"/>
                  <!--   <xsl:value-of select="OldSecFee + OldOtherBrokerFees + OldClearingBrokerFee + OldMiscFees + OldOccFee + OldOrfFee + OldStampDuty + OldTransactionLevy"/> -->
                </OTH-FEE>

                <INT-AMT>
                  <xsl:value-of select="''"/>
                </INT-AMT>

                <INT-TAX>
                  <xsl:value-of select="''"/>
                </INT-TAX>

                <INT-DAY>
                  <xsl:value-of select="''"/>
                </INT-DAY>



                <TRS-GRO-AMT>

                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                      <xsl:value-of select="my:RoundOff(OldAvgPrice * OldExecutedQuantity)"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="format-number(OldAvgPrice * OldExecutedQuantity,'#.##')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </TRS-GRO-AMT>
                <xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
                <TRS-NET-AMT>

                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                      <xsl:value-of select="my:RoundOff($varOldNetAmount)"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="format-number($varOldNetAmount,'#.##')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </TRS-NET-AMT>

                <xsl:variable name="varFXRate">
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol != OldSettlCurrency">
                      <xsl:value-of select="FXRate_Taxlot"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="1"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <xsl:variable name="varNetAmountBase">
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol = OldSettlCurrency">
                      <xsl:value-of select="$varOldNetAmount"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$varOldNetAmount * $varFXRate"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <SET-NET-AMT>

                  <xsl:choose>
                    <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                      <xsl:value-of select="my:RoundOff($varOldNetAmount)"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="format-number($varOldNetAmount,'#.##')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SET-NET-AMT>


                <FEE-AMT>
                  <xsl:value-of select="''"/>
                </FEE-AMT>

                <FM-TXT>
                  <xsl:value-of select="''"/>
                </FM-TXT>

                <COM-TXT>
                  <xsl:value-of select="''"/>
                </COM-TXT>

                <EVEN-TYP>
                  <xsl:value-of select="'N'"/>
                </EVEN-TYP>

                <PSET-BIC>
                  <xsl:choose>

                    <xsl:when test="Exchange ='FRA'">
                      <xsl:value-of select="'DAKVDEFFXXX'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </PSET-BIC>

                <PSET-PAYS>
                  <xsl:value-of select="''"/>
                </PSET-PAYS>

                <CUS-FEE>
                  <xsl:value-of select="''"/>
                </CUS-FEE>

                <SCUS-FEE>
                  <xsl:value-of select="''"/>
                </SCUS-FEE>

                <STAMP-FEE>
                  <xsl:value-of select="''"/>
                </STAMP-FEE>

                <OPC-IO-TOT>
                  <xsl:value-of select="''"/>
                </OPC-IO-TOT>

                <OPC-IO-REF>
                  <xsl:value-of select="''"/>
                </OPC-IO-REF>

                <OPC-IO-ACQ>
                  <xsl:value-of select="''"/>
                </OPC-IO-ACQ>

                <FACTOR>
                  <xsl:value-of select="''"/>
                </FACTOR>

                <QTE-TYP>
                  <xsl:value-of select="'UNIT'"/>
                </QTE-TYP>

                <VAT>
                  <xsl:value-of select="''"/>
                </VAT>

                <TYP-TAU>
                  <xsl:value-of select="''"/>
                </TYP-TAU>

                <SET-FLG>
                  <xsl:value-of select="''"/>
                </SET-FLG>

                <DEL-COD>
                  <xsl:value-of select="'A'"/>
                </DEL-COD>

                <DEP-FIN-REF>
                  <xsl:value-of select="''"/>
                </DEP-FIN-REF>

                <DEP-FIN-DES>
                  <xsl:value-of select="''"/>
                </DEP-FIN-DES>

                <DEP-FIN-ACC>
                  <xsl:value-of select="''"/>
                </DEP-FIN-ACC>

                <PTG-CSH-ACC-TYP>
                  <xsl:value-of select="''"/>
                </PTG-CSH-ACC-TYP>

                <PTG-CSH-ACC-NBR>
                  <xsl:value-of select="''"/>
                </PTG-CSH-ACC-NBR>

                <SPREAD>
                  <xsl:value-of select="''"/>
                </SPREAD>

                <MARKET-CLAIM-OPT-OUT>
                  <xsl:value-of select="''"/>
                </MARKET-CLAIM-OPT-OUT>

                <EX-CUM-COUPON>
                  <xsl:value-of select="''"/>
                </EX-CUM-COUPON>

                <PSAF-BIC>
                  <xsl:value-of select="''"/>
                </PSAF-BIC>

                <INDIC1>
                  <xsl:value-of select="'SETR//TRAD'"/>
                </INDIC1>

                <BLB-CODE>
                  <xsl:value-of select="''"/>
                </BLB-CODE>

                <INDIC4>
                  <xsl:value-of select="''"/>
                </INDIC4>

                <CUR-CHG>
                  <xsl:value-of select="''"/>
                </CUR-CHG>

                <INDIC5>
                  <xsl:value-of select="''"/>
                </INDIC5>

                <INDIC6>
                  <xsl:value-of select="''"/>
                </INDIC6>

                <INDIC7>
                  <xsl:value-of select="''"/>
                </INDIC7>

                <INDIC8>
                  <xsl:value-of select="''"/>
                </INDIC8>

                <AMORQTY>
                  <xsl:value-of select="''"/>
                </AMORQTY>

                <REPMATDAT>
                  <xsl:value-of select="''"/>
                </REPMATDAT>

                <REPRATDAT>
                  <xsl:value-of select="''"/>
                </REPRATDAT>

                <REPRATTYP>
                  <xsl:value-of select="''"/>
                </REPRATTYP>

                <REPINTMIC>
                  <xsl:value-of select="''"/>
                </REPINTMIC>

                <REPREVIND>
                  <xsl:value-of select="''"/>
                </REPREVIND>

                <REPLEGIND>
                  <xsl:value-of select="''"/>
                </REPLEGIND>

                <REPMDMIND>
                  <xsl:value-of select="''"/>
                </REPMDMIND>

                <REPINTIND>
                  <xsl:value-of select="''"/>
                </REPINTIND>

                <REPREFSEC>
                  <xsl:value-of select="''"/>
                </REPREFSEC>

                <REPREFREP>
                  <xsl:value-of select="''"/>
                </REPREFREP>

                <REPRATREP>
                  <xsl:value-of select="''"/>
                </REPRATREP>

                <REPRATVAR>
                  <xsl:value-of select="''"/>
                </REPRATVAR>

                <REPSPRRAT>
                  <xsl:value-of select="''"/>
                </REPSPRRAT>

                <REPPRCRAT>
                  <xsl:value-of select="''"/>
                </REPPRCRAT>

                <REPLOAMRG>
                  <xsl:value-of select="''"/>
                </REPLOAMRG>

                <REPSECHRC>
                  <xsl:value-of select="''"/>
                </REPSECHRC>

                <REPNBRTCD>
                  <xsl:value-of select="''"/>
                </REPNBRTCD>

                <REPNBRNCO>
                  <xsl:value-of select="''"/>
                </REPNBRNCO>

                <REPFRFAMT>
                  <xsl:value-of select="''"/>
                </REPFRFAMT>

                <REPFRFCCY>
                  <xsl:value-of select="''"/>
                </REPFRFCCY>

                <REPTERAMT>
                  <xsl:value-of select="''"/>
                </REPTERAMT>

                <REPTERCCY>
                  <xsl:value-of select="''"/>
                </REPTERCCY>

                <REPPRMAMT>
                  <xsl:value-of select="''"/>
                </REPPRMAMT>

                <REPPRMCCY>
                  <xsl:value-of select="''"/>
                </REPPRMCCY>

                <REPACRAMT>
                  <xsl:value-of select="''"/>
                </REPACRAMT>

                <REPACRCCY>
                  <xsl:value-of select="''"/>
                </REPACRCCY>

                <REPDEAAMT>
                  <xsl:value-of select="''"/>
                </REPDEAAMT>

                <REPDEACCY>
                  <xsl:value-of select="''"/>
                </REPDEACCY>

                <REPTPCAMT>
                  <xsl:value-of select="''"/>
                </REPTPCAMT>

                <REPTPCCCY>
                  <xsl:value-of select="''"/>
                </REPTPCCCY>

                <REPLEGNAR>
                  <xsl:value-of select="''"/>
                </REPLEGNAR>

                <COMFIA>
                  <xsl:value-of select="''"/>
                </COMFIA>

                <COMTRD>
                  <xsl:value-of select="''"/>
                </COMTRD>


                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>
            </xsl:if>

            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select="'false'"/>
              </RowHeader>

              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>

              <TaxLotState>
                <xsl:value-of select ="'Allocated'"/>
              </TaxLotState>

              <xsl:variable name="varTaxlotState">
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="'CREATE'"/>
                  </xsl:when>

                  <xsl:when test="TaxLotState='Deleted'">
                    <xsl:value-of select ="'DELETE'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <ACTION>
                <xsl:value-of select ="'CREATE'"/>
              </ACTION>

              <EMETTEUR>
                <xsl:value-of select="'UKJUPITER'"/>
              </EMETTEUR>



              <REF-EXT>
                <xsl:value-of select="concat(PBUniqueID,'A')"/>
              </REF-EXT>

              <INTERNAL_ORIGID>
                <xsl:value-of select="''"/>
              </INTERNAL_ORIGID>

              <INTERNAL_ID>
                <xsl:value-of select="''"/>
              </INTERNAL_ID>

              <INTERNAL_STATUS>
                <xsl:value-of select="''"/>
              </INTERNAL_STATUS>

              <EXTERNAL_ORIGID>
                <xsl:value-of select="''"/>
              </EXTERNAL_ORIGID>

              <EXTERNAL_ID>
                <xsl:value-of select="''"/>
              </EXTERNAL_ID>

              <EXTERNAL_STATUS>
                <xsl:value-of select="''"/>
              </EXTERNAL_STATUS>

              <DATE_OUT>
                <xsl:value-of select="''"/>
              </DATE_OUT>

              <TIME_OUT>
                <xsl:value-of select="''"/>
              </TIME_OUT>

              <ERROR_MESSAGE>
                <xsl:value-of select="''"/>
              </ERROR_MESSAGE>

              <xsl:variable name="varSide1">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'SEL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <OPE_TYP>
                <xsl:value-of select="$varSide1"/>
              </OPE_TYP>

              <MGP>
                <xsl:value-of select="'30355'"/>
              </MGP>

              <CASH-COR>
                <xsl:value-of select="''"/>
              </CASH-COR>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'RBC'"/>
              </xsl:variable>
              <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
              <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CurrencySymbol"/>


              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_COUNTERPARTY_Name]/@BICCode"/>
              </xsl:variable>

              <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerName">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerName"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
              </xsl:variable>
              <BRK-REF>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC != ''">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYUS33'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BRK-REF>

              <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
              </xsl:variable>

              <xsl:variable name="Broker">
                <xsl:choose>

                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <BRK-NAM>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerName!=''">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerName"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BRK-NAM>

              <BRK-ACC>
                <xsl:value-of select="''"/>
              </BRK-ACC>
              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
              </xsl:variable>

              <SUB-CLR-ACC>
                <xsl:choose>

                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'0161'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-CLR-ACC>



              <SUB-CLR-COD>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-CLR-COD>



              <SUB-REF>
                <xsl:choose>

                  <xsl:when test="Exchange ='FRA'">
                    <xsl:value-of select="'PARBDEFFXXX'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType = 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-REF>

              <SUB-DES>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerName!=''">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerName"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SUB-DES>

              <NIV-CPT1>
                <xsl:value-of select="''"/>
              </NIV-CPT1>

              <xsl:variable name="varCLRACC">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'0161'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <CLR-ACC>
                <xsl:choose>

                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'0161'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CLR-ACC>

              <xsl:variable name="varCLRCod">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <CLR-COD>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                    <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CLR-COD>

              <NIV-BIC3>
                <xsl:value-of select="''"/>
              </NIV-BIC3>

              <NIV-LIB3>
                <xsl:value-of select="''"/>
              </NIV-LIB3>

              <NIV-CPT3>
                <xsl:value-of select="''"/>
              </NIV-CPT3>

              <NIV-CLR-ACC3>
                <xsl:value-of select="''"/>
              </NIV-CLR-ACC3>

              <NIV-CLR-COD3>
                <xsl:value-of select="''"/>
              </NIV-CLR-COD3>

              <NIV-BIC4>
                <xsl:value-of select="''"/>
              </NIV-BIC4>

              <NIV-LIB4>
                <xsl:value-of select="''"/>
              </NIV-LIB4>

              <NIV-CPT4>
                <xsl:value-of select="''"/>
              </NIV-CPT4>

              <NIV-CLR-ACC4>
                <xsl:value-of select="''"/>
              </NIV-CLR-ACC4>

              <NIV-CLR-COD4>
                <xsl:value-of select="''"/>
              </NIV-CLR-COD4>

              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

              <TRA-DAT>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TRA-DAT>



              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SET-DAT>
                <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>

              </SET-DAT>
              <TRS-CUR>
                <xsl:value-of select="CurrencySymbol"/>
              </TRS-CUR>

              <xsl:variable name="varSecurityIDType">
                <xsl:choose>
                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'IC'"/>
                  </xsl:when>


                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="'GB'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="'US'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SEC-TYP>
                <xsl:value-of select="$varSecurityIDType"/>
              </SEC-TYP>

              <xsl:variable name="varSecurityID">
                <xsl:choose>
                  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="ISIN"/>
                  </xsl:when>


                  <xsl:when test="SEDOL!=''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="CUSIP!=''">
                    <xsl:value-of select="CUSIP"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SEC-COD>
                <xsl:value-of select="$varSecurityID"/>
              </SEC-COD>

              <SEC-DES>
                <xsl:value-of select="CompanyName"/>
              </SEC-DES>

              <QTY>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff(OrderQty)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number(OrderQty,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </QTY>

              <PRI>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff(AvgPrice)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number(AvgPrice,'#.######')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PRI>

              <INT-MOD>
                <xsl:value-of select="''"/>
              </INT-MOD>

              <FEE-CUR>
                <xsl:value-of select="CurrencySymbol"/>
              </FEE-CUR>

              <STK-EXC>
                <xsl:value-of select="''"/>
              </STK-EXC>

              <SET-CUR>
                <xsl:value-of select="CurrencySymbol"/>
              </SET-CUR>

              <CHG-RAT>
                <xsl:value-of select="''"/>
              </CHG-RAT>
              <xsl:variable name="varCommission">
                <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
              </xsl:variable>
              <BRK-FEE>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff($varCommission)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varCommission,'0.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </BRK-FEE>

              <TAX-FEE>
                <xsl:value-of select="''"/>
              </TAX-FEE>

              <OTH-FEE>
                <xsl:value-of select="format-number((OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingBrokerFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee + StampDuty),'0.##')"/>
                <!--  <xsl:value-of select="SecFee + OtherBrokerFees + ClearingBrokerFee + MiscFees + OccFee + OrfFee + StampDuty + TransactionLevy"/> -->
              </OTH-FEE>

              <INT-AMT>
                <xsl:value-of select="''"/>
              </INT-AMT>

              <INT-TAX>
                <xsl:value-of select="''"/>
              </INT-TAX>

              <INT-DAY>
                <xsl:value-of select="''"/>
              </INT-DAY>


              <TRS-GRO-AMT>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff(AvgPrice * OrderQty)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number(AvgPrice * OrderQty,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TRS-GRO-AMT>

              <TRS-NET-AMT>

                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff($varNetamount)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varNetamount,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TRS-NET-AMT>

              <xsl:variable name="varFXRate">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol != SettlCurrency">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <xsl:variable name="varNetAmountBase">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol = SettlCurrency">
                    <xsl:value-of select="$varNetamount"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varNetamount * $varFXRate"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SET-NET-AMT>


                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='JPY' or CurrencySymbol ='KRW' or CurrencySymbol ='VND'">
                    <xsl:value-of select="my:RoundOff($varNetamount)"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="format-number($varNetamount,'#.##')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SET-NET-AMT>


              <FEE-AMT>
                <xsl:value-of select="''"/>
              </FEE-AMT>

              <FM-TXT>
                <xsl:value-of select="''"/>
              </FM-TXT>

              <COM-TXT>
                <xsl:value-of select="''"/>
              </COM-TXT>

              <EVEN-TYP>
                <xsl:value-of select="'N'"/>
              </EVEN-TYP>

              <PSET-BIC>
                <xsl:choose>

                  <xsl:when test="Exchange ='FRA'">
                    <xsl:value-of select="'DAKVDEFFXXX'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PSET-BIC>

              <PSET-PAYS>
                <xsl:value-of select="''"/>
              </PSET-PAYS>

              <CUS-FEE>
                <xsl:value-of select="''"/>
              </CUS-FEE>

              <SCUS-FEE>
                <xsl:value-of select="''"/>
              </SCUS-FEE>

              <STAMP-FEE>
                <xsl:value-of select="''"/>
              </STAMP-FEE>

              <OPC-IO-TOT>
                <xsl:value-of select="''"/>
              </OPC-IO-TOT>

              <OPC-IO-REF>
                <xsl:value-of select="''"/>
              </OPC-IO-REF>

              <OPC-IO-ACQ>
                <xsl:value-of select="''"/>
              </OPC-IO-ACQ>

              <FACTOR>
                <xsl:value-of select="''"/>
              </FACTOR>

              <QTE-TYP>
                <xsl:value-of select="'UNIT'"/>
              </QTE-TYP>

              <VAT>
                <xsl:value-of select="''"/>
              </VAT>

              <TYP-TAU>
                <xsl:value-of select="''"/>
              </TYP-TAU>

              <SET-FLG>
                <xsl:value-of select="''"/>
              </SET-FLG>

              <DEL-COD>
                <xsl:value-of select="'A'"/>
              </DEL-COD>

              <DEP-FIN-REF>
                <xsl:value-of select="''"/>
              </DEP-FIN-REF>

              <DEP-FIN-DES>
                <xsl:value-of select="''"/>
              </DEP-FIN-DES>

              <DEP-FIN-ACC>
                <xsl:value-of select="''"/>
              </DEP-FIN-ACC>

              <PTG-CSH-ACC-TYP>
                <xsl:value-of select="''"/>
              </PTG-CSH-ACC-TYP>

              <PTG-CSH-ACC-NBR>
                <xsl:value-of select="''"/>
              </PTG-CSH-ACC-NBR>

              <SPREAD>
                <xsl:value-of select="''"/>
              </SPREAD>

              <MARKET-CLAIM-OPT-OUT>
                <xsl:value-of select="''"/>
              </MARKET-CLAIM-OPT-OUT>

              <EX-CUM-COUPON>
                <xsl:value-of select="''"/>
              </EX-CUM-COUPON>

              <PSAF-BIC>
                <xsl:value-of select="''"/>
              </PSAF-BIC>

              <INDIC1>
                <xsl:value-of select="'SETR//TRAD'"/>
              </INDIC1>

              <BLB-CODE>
                <xsl:value-of select="''"/>
              </BLB-CODE>

              <INDIC4>
                <xsl:value-of select="''"/>
              </INDIC4>

              <CUR-CHG>
                <xsl:value-of select="''"/>
              </CUR-CHG>

              <INDIC5>
                <xsl:value-of select="''"/>
              </INDIC5>

              <INDIC6>
                <xsl:value-of select="''"/>
              </INDIC6>

              <INDIC7>
                <xsl:value-of select="''"/>
              </INDIC7>

              <INDIC8>
                <xsl:value-of select="''"/>
              </INDIC8>

              <AMORQTY>
                <xsl:value-of select="''"/>
              </AMORQTY>

              <REPMATDAT>
                <xsl:value-of select="''"/>
              </REPMATDAT>

              <REPRATDAT>
                <xsl:value-of select="''"/>
              </REPRATDAT>

              <REPRATTYP>
                <xsl:value-of select="''"/>
              </REPRATTYP>

              <REPINTMIC>
                <xsl:value-of select="''"/>
              </REPINTMIC>

              <REPREVIND>
                <xsl:value-of select="''"/>
              </REPREVIND>

              <REPLEGIND>
                <xsl:value-of select="''"/>
              </REPLEGIND>

              <REPMDMIND>
                <xsl:value-of select="''"/>
              </REPMDMIND>

              <REPINTIND>
                <xsl:value-of select="''"/>
              </REPINTIND>

              <REPREFSEC>
                <xsl:value-of select="''"/>
              </REPREFSEC>

              <REPREFREP>
                <xsl:value-of select="''"/>
              </REPREFREP>

              <REPRATREP>
                <xsl:value-of select="''"/>
              </REPRATREP>

              <REPRATVAR>
                <xsl:value-of select="''"/>
              </REPRATVAR>

              <REPSPRRAT>
                <xsl:value-of select="''"/>
              </REPSPRRAT>

              <REPPRCRAT>
                <xsl:value-of select="''"/>
              </REPPRCRAT>

              <REPLOAMRG>
                <xsl:value-of select="''"/>
              </REPLOAMRG>

              <REPSECHRC>
                <xsl:value-of select="''"/>
              </REPSECHRC>

              <REPNBRTCD>
                <xsl:value-of select="''"/>
              </REPNBRTCD>

              <REPNBRNCO>
                <xsl:value-of select="''"/>
              </REPNBRNCO>

              <REPFRFAMT>
                <xsl:value-of select="''"/>
              </REPFRFAMT>

              <REPFRFCCY>
                <xsl:value-of select="''"/>
              </REPFRFCCY>

              <REPTERAMT>
                <xsl:value-of select="''"/>
              </REPTERAMT>

              <REPTERCCY>
                <xsl:value-of select="''"/>
              </REPTERCCY>

              <REPPRMAMT>
                <xsl:value-of select="''"/>
              </REPPRMAMT>

              <REPPRMCCY>
                <xsl:value-of select="''"/>
              </REPPRMCCY>

              <REPACRAMT>
                <xsl:value-of select="''"/>
              </REPACRAMT>

              <REPACRCCY>
                <xsl:value-of select="''"/>
              </REPACRCCY>

              <REPDEAAMT>
                <xsl:value-of select="''"/>
              </REPDEAAMT>

              <REPDEACCY>
                <xsl:value-of select="''"/>
              </REPDEACCY>

              <REPTPCAMT>
                <xsl:value-of select="''"/>
              </REPTPCAMT>

              <REPTPCCCY>
                <xsl:value-of select="''"/>
              </REPTPCCCY>

              <REPLEGNAR>
                <xsl:value-of select="''"/>
              </REPLEGNAR>

              <COMFIA>
                <xsl:value-of select="''"/>
              </COMFIA>

              <COMTRD>
                <xsl:value-of select="''"/>
              </COMTRD>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>

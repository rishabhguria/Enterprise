<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>


        <entry_type>
          <xsl:value-of select="'entry_type'"/>
        </entry_type>

        <trd_type>
          <xsl:value-of select="'trd_type'"/>
        </trd_type>

        <trade_dt>
          <xsl:value-of select="'trade_dt'"/>
        </trade_dt>
      
        <settle_dt>
          <xsl:value-of select="'settle_dt'"/>
        </settle_dt>

        <exec_dt>
          <xsl:value-of select="'exec_dt'"/>
        </exec_dt>

        <symbol>
          <xsl:value-of select="'symbol'"/>
        </symbol>

        <qty>
          <xsl:value-of select="'qty'"/>
        </qty>

        <price>
          <xsl:value-of select="'price'"/>
        </price>

        <g_amt>
          <xsl:value-of select="'g_amt'"/>
        </g_amt>

        <comm>
          <xsl:value-of select="'comm'"/>
        </comm>

        <sec_fee>
          <xsl:value-of select="'sec_fee'"/>
        </sec_fee>

        <exch_fee>
          <xsl:value-of select="'exch_fee'"/>
        </exch_fee>

        <clr_fee>
          <xsl:value-of select="'clr_fee'"/>
        </clr_fee>

        <ecn_fee>
          <xsl:value-of select="'ecn_fee'"/>
        </ecn_fee>

        <brk_fee>
          <xsl:value-of select="'brk_fee'"/>
        </brk_fee>

        <occ_fee>
          <xsl:value-of select="'occ_fee'"/>
        </occ_fee>

        <oth_fee>
          <xsl:value-of select="'oth_fee'"/>
        </oth_fee>

     

        <corr>
          <xsl:value-of select="'corr'"/>
        </corr>

        <office>
          <xsl:value-of select="'office'"/>
        </office>


        <acct_no>
          <xsl:value-of select ="'acct_no'"/>
        </acct_no>

        <acct_type>
          <xsl:value-of select="'acct_type'"/>
        </acct_type>

        <sub_acct_noSub>
          <xsl:value-of select="'sub_acct_noSub'"/>
        </sub_acct_noSub>

        <contra_code>
          <xsl:value-of select="'contra_code'"/>
        </contra_code>

        <contra_corr>
          <xsl:value-of select="'contra_corr'"/>
        </contra_corr>

       

        <contra_office>
          <xsl:value-of select="'contra_office'"/>
        </contra_office>


        <contra_acct_no>
          <xsl:value-of select="'contra_acct_no'"/>
        </contra_acct_no>

        <contra_acct_type>
          <xsl:value-of select="'contra_acct_type'"/>
        </contra_acct_type>

        <contra_sub_acct_no>
          <xsl:value-of select="'contra_sub_acct_no'"/>
        </contra_sub_acct_no>

        <blot_exch_cd>
          <xsl:value-of select="'blot_exch_cd'"/>
        </blot_exch_cd>

        <blot_clr_typ>
          <xsl:value-of select="'blot_clr_typ'"/>
        </blot_clr_typ>

        <blot_method>
          <xsl:value-of select="'blot_method'"/>
        </blot_method>

        <trd_tag>
          <xsl:value-of select="'trd_tag'"/>
        </trd_tag>

        <descr>
          <xsl:value-of select="'descr'"/>
        </descr>

        <memo1>
          <xsl:value-of select="'memo1'"/>
        </memo1>

        <memo2>
          <xsl:value-of select="'memo2'"/>
        </memo2>

        <tax_lot>
          <xsl:value-of select="'tax_lot'"/>
        </tax_lot>

        <lot_tr_no>
          <xsl:value-of select="'lot_tr_no'"/>
        </lot_tr_no>

        <buy_interest>
          <xsl:value-of select="'buy_interest'"/>
        </buy_interest>

        <finc_yield>
          <xsl:value-of select="'finc_yield'"/>
        </finc_yield>

        <yield_type>
          <xsl:value-of select="'yield_type'"/>
        </yield_type>

        <capacity>
          <xsl:value-of select="'capacity'"/>
        </capacity>

        <ac_grp_cd>
          <xsl:value-of select="'ac_grp_cd'"/>
        </ac_grp_cd>

        <trailer_codes>
          <xsl:value-of select="'trailer_codes'"/>
        </trailer_codes>

        <rr_cd>
          <xsl:value-of select="'rr_cd'"/>
        </rr_cd>

        <ad_cd>
          <xsl:value-of select="'ad_cd'"/>
        </ad_cd>

        <an_cd>
          <xsl:value-of select="'an_cd'"/>
        </an_cd>

        <tr_cd>
          <xsl:value-of select="'tr_cd'"/>
        </tr_cd>

        <currency>
          <xsl:value-of select="'currency'"/>
        </currency>

        <set_currency>
          <xsl:value-of select="'set_currency'"/>
        </set_currency>

        <set_country>
          <xsl:value-of select="'set_country'"/>
        </set_country>

        <set_location>
          <xsl:value-of select="'set_location'"/>
        </set_location>

        <reported_price>
          <xsl:value-of select="'reported_price'"/>
        </reported_price>

        <sales_credit>
          <xsl:value-of select="'sales_credit'"/>
        </sales_credit>

        <discretion_flg>
          <xsl:value-of select="'discretion_flg'"/>
        </discretion_flg>

        <solicited>
          <xsl:value-of select="'solicited'"/>
        </solicited>

        <comm_type>
          <xsl:value-of select="'comm_type'"/>
        </comm_type>

        <taxlot_tr_no>
          <xsl:value-of select="'taxlot_tr_no'"/>
        </taxlot_tr_no>

        <cl_order_id>
          <xsl:value-of select="'cl_order_id'"/>
        </cl_order_id>

        <order_id>
          <xsl:value-of select="'order_id'"/>
        </order_id>

        <exec_id>
          <xsl:value-of select="'exec_id'"/>
        </exec_id>

        <memo3>
          <xsl:value-of select="'memo3'"/>
        </memo3>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>


      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <entry_type>
            <xsl:value-of select="'TRD'"/>
          </entry_type>

          <trd_type>
            <xsl:choose>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'OB'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'OS'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CB'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </trd_type>
          
          <xsl:variable name="varSTradeDate">
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </xsl:variable>
          <trade_dt>
            <xsl:value-of select="$varSTradeDate"/>
          </trade_dt>
<xsl:variable name="varSettlementDate">
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </xsl:variable>
          <settle_dt>
            <xsl:value-of select="$varSettlementDate"/>
          </settle_dt>

          <exec_dt>
            <xsl:value-of select="''"/>
          </exec_dt>

          <symbol>
            <xsl:value-of select="OSIOptionSymbol"/>
          </symbol>

          <qty>
            <xsl:value-of select="AllocatedQty"/>
          </qty>

          <price>
            <xsl:value-of select="AveragePrice"/>
          </price>

          <g_amt>
            <xsl:value-of select="''"/>
          </g_amt>

          <comm>
            <!-- <xsl:choose> -->
              <!-- <xsl:when test="number(CommissionCharged)"> -->
                <!-- <xsl:value-of select="AllocatedQty * number(0.5)"/> -->
              <!-- </xsl:when> -->
              <!-- <xsl:otherwise> -->
                <xsl:value-of select="AllocatedQty * number(0.5)"/>
              <!-- </xsl:otherwise> -->
            <!-- </xsl:choose> -->
          </comm>

          <sec_fee>
            <xsl:value-of select="''"/>
          </sec_fee>

          <exch_fee>
            <xsl:value-of select="''"/>
          </exch_fee>

          <clr_fee>
            <xsl:value-of select="''"/>
          </clr_fee>

          <ecn_fee>
            <xsl:value-of select="''"/>
          </ecn_fee>

          <brk_fee>
            <xsl:value-of select="''"/>
          </brk_fee>

          <occ_fee>
            <xsl:value-of select="''"/>
          </occ_fee>

          <oth_fee>
            <xsl:value-of select="''"/>
          </oth_fee>

          <xsl:variable name = "PB_NAME">
            <xsl:value-of select="'1'"/>
          </xsl:variable>

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_CORR_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CorrCodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@Pranacorr"/>
          </xsl:variable>

          <corr>
            <xsl:value-of select="$PRANA_CORR_NAME"/>
          </corr>

          <xsl:variable name ="PRANA_OFFICE_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CorrCodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@office"/>
          </xsl:variable>
          <office>
            <xsl:value-of select="$PRANA_OFFICE_NAME"/>
          </office>

          <xsl:variable name ="PRANA_ACC_NO_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CorrCodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@acct_no"/>
          </xsl:variable>

          <acct_no>
            <xsl:value-of select ="$PRANA_ACC_NO_NAME"/>
          </acct_no>

          <xsl:variable name ="PRANA_ACC_TYPE_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CorrCodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@acct_type"/>
          </xsl:variable>
          <acct_type>
            <xsl:value-of select="$PRANA_ACC_TYPE_NAME"/>
          </acct_type>

          <sub_acct_noSub>
            <xsl:value-of select="''"/>
          </sub_acct_noSub>

          <contra_code>
            <xsl:value-of select="'VHTO'"/>
          </contra_code>


          <xsl:variable name ="PRANA_CONTRA_CORR_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_Contra_CodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@contra_corr"/>
          </xsl:variable>
          <contra_corr>
            <xsl:value-of select="$PRANA_CONTRA_CORR_NAME"/>
          </contra_corr>

          <xsl:variable name ="PRANA_OFFICE_CORR_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_Contra_CodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@contra_office"/>
          </xsl:variable>

          <contra_office>
            <xsl:value-of select="$PRANA_OFFICE_CORR_NAME"/>
          </contra_office>


          <xsl:variable name ="PRANA_CONTRA_ACCT_NO_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_Contra_CodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@contra_acct_no"/>
          </xsl:variable>
          <contra_acct_no>
            <xsl:value-of select="$PRANA_CONTRA_ACCT_NO_NAME"/>
          </contra_acct_no>

          <xsl:variable name ="PRANA_CONTRA_ACCT_TYPE_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_Contra_CodeMapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@PranaAccount=$PB_FUND_NAME]/@contra_acct_type"/>
          </xsl:variable>
          <contra_acct_type>
            <xsl:value-of select="$PRANA_CONTRA_ACCT_TYPE_NAME"/>
          </contra_acct_type>

          <contra_sub_acct_no>
            <xsl:value-of select="''"/>
          </contra_sub_acct_no>

          <blot_exch_cd>
            <xsl:value-of select="'00'"/>
          </blot_exch_cd>

          <blot_clr_typ>
            <xsl:value-of select="'X5'"/>
          </blot_clr_typ>

          <blot_method>
            <xsl:value-of select="'CM'"/>
          </blot_method>

          <trd_tag>
            <xsl:value-of select="PBUniqueID"/>
          </trd_tag>

          <descr>
            <xsl:value-of select="''"/>
          </descr>

          <memo1>
            <xsl:value-of select="''"/>
          </memo1>

          <memo2>
            <xsl:value-of select="''"/>
          </memo2>

          <tax_lot>
            <xsl:value-of select="''"/>
          </tax_lot>

          <lot_tr_no>
            <xsl:value-of select="''"/>
          </lot_tr_no>

          <buy_interest>
            <xsl:value-of select="''"/>
          </buy_interest>

          <finc_yield>
            <xsl:value-of select="''"/>
          </finc_yield>

          <yield_type>
            <xsl:value-of select="''"/>
          </yield_type>

          <capacity>
            <xsl:value-of select="'A'"/>
          </capacity>

          <ac_grp_cd>
            <xsl:value-of select="''"/>
          </ac_grp_cd>

          <trailer_codes>
            <xsl:value-of select="''"/>
          </trailer_codes>

          <rr_cd>
            <xsl:value-of select="''"/>
          </rr_cd>

          <ad_cd>
            <xsl:value-of select="''"/>
          </ad_cd>

          <an_cd>
            <xsl:value-of select="''"/>
          </an_cd>

          <tr_cd>
            <xsl:value-of select="''"/>
          </tr_cd>

          <currency>
            <xsl:value-of select="''"/>
          </currency>

          <set_currency>
            <xsl:value-of select="''"/>
          </set_currency>

          <set_country>
            <xsl:value-of select="''"/>
          </set_country>

          <set_location>
            <xsl:value-of select="''"/>
          </set_location>

          <reported_price>
            <xsl:value-of select="''"/>
          </reported_price>

          <sales_credit>
            <xsl:value-of select="''"/>
          </sales_credit>

          <discretion_flg>
            <xsl:value-of select="''"/>
          </discretion_flg>

          <solicited>
            <xsl:value-of select="''"/>
          </solicited>

          <comm_type>
            <xsl:value-of select="''"/>
          </comm_type>

          <taxlot_tr_no>
            <xsl:value-of select="''"/>
          </taxlot_tr_no>

          <cl_order_id>
            <xsl:value-of select="''"/>
          </cl_order_id>

          <order_id>
            <xsl:value-of select="''"/>
          </order_id>

          <exec_id>
            <xsl:value-of select="''"/>
          </exec_id>

          <memo3>
            <xsl:value-of select="''"/>
          </memo3>
        
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
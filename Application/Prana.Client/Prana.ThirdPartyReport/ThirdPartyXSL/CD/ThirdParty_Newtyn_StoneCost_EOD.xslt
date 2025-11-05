<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
      <xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail">
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/TaxLotID != TaxLotID)">
          <!-- ...buid a Group for this node_id -->
          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_TaxLotID">
              <xsl:value-of select="TaxLotID" />
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
      </xsl:for-each>
    </Groups>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_TaxLotID" />

    <xsl:variable name="varClosingMethod">
      <xsl:choose>
        <xsl:when test="ClosingAlgo = 'HIFO'">
          <xsl:value-of select="''"/>
        </xsl:when>
        <xsl:when test="ClosingAlgo = 'MANUAL'">
          <xsl:value-of select="'Specified'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="ClosingAlgo"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

	  <xsl:variable name ="varExpirationDate">
		  <xsl:if test ="Asset = 'EquityOption' or Asset = 'FutureOption' or Asset = 'Future' or Asset = 'FixedIncome'">
			  <xsl:value-of select ="ExpirationDate"/>
		  </xsl:if>
	  </xsl:variable>


	  <xsl:variable name="varQ1">
		  <xsl:choose>
			  <xsl:when test= "ClosingAlgo != '*' and ClosingAlgo != '' and ClosingAlgo != 'HIFO'">
				  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/ClosedQty"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varQ2">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ3">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ4">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ5">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ6">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=6]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ7">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=7]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ8">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=8]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ9">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=9]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ10">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=10]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>


	  <xsl:variable name="varClosedPrice1">
		  <xsl:choose>
			  <xsl:when test= "ClosingAlgo != '*' and ClosingAlgo != '' and ClosingAlgo != 'HIFO'">
				  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/OpenPriceAgainstClosing"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice2">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice3">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice4">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice5">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice6">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=6]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice7">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=7]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice8">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=8]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice9">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=9]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedPrice10">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=10]/OpenPriceAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>



	  <xsl:variable name="varClosedAgainstDate1">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate2">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate3">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate4">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate5">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate6">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=6]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate7">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=7]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate8">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=8]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate9">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=9]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate10">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=10]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name ="UDA_Country">
		  <xsl:value-of select ="UDACountryName"/>
	  </xsl:variable>
	  <xsl:variable name ="PB_SettleCurrency">
		  <xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name='ALL']/SymbolData[@UDACountry=$UDA_Country]/@SettleCurrency"/>
	  </xsl:variable>

	  <xsl:variable name ="varAvgPrice">
		  <xsl:choose>
			  <xsl:when test ="CounterParty = 'GWEP'">
				  <xsl:value-of  select="format-number(AveragePrice,'#.0000')"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of  select="AveragePrice"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varClientAccount">
		  <xsl:choose>
			  <xsl:when test ="AccountName = 'BNP NP 491-00040'">
				  <xsl:value-of select ="'BNP Newtyn Partners LP:491-00040'"/>
			  </xsl:when>
			  <xsl:when test ="AccountName = 'BNP NTE 491-00041'">
				  <xsl:value-of select ="'BNP Newtyn TE Partners LP:491-00041'"/>
			  </xsl:when>
			  <xsl:when test ="AccountName = 'GS NP 002486561'">
				  <xsl:value-of select ="'GS Newtyn Partners LP:002486561'"/>
			  </xsl:when>
			  <xsl:when test ="AccountName = 'GS NTE 002486553'">
				  <xsl:value-of select ="'GS Newtyn TE Partners LP:002486553'"/>
			  </xsl:when>
			  <xsl:when test ="AccountName = 'Citi NP 522-91K57'">
				  <xsl:value-of select ="'Citi Newtyn Partners LP:522-91K57'"/>
			  </xsl:when>
			  <xsl:when test ="AccountName = 'Citi NTE 522-91K58'">
				  <xsl:value-of select ="'Citi Newtyn TE Partners LP:522-91K58'"/>
			  </xsl:when>
		  </xsl:choose>
	  </xsl:variable>

	  <Group 
			  AccountNumber="{$varClientAccount}" Strategy ="{Strategy}" TradeDate = "{TradeDate}" SettleDate = "{SettlementDate}" Side="{Side}"
        Ticker ="{Symbol}" CUSIP ="{CUSIP}" Issuer="" OtherDescription ="{FullSecurityName}" Coupon="{Coupon}" Maturity="{$varExpirationDate}"
        OriginalFace="" TradeQuantity="{AllocatedQty}" Factor="{AssetMultiplier}" Price="{$varAvgPrice}" TradeFXRate="{ForexRate}"
        TradeCurrency="{CurrencySymbol}" SettleCurrency="{$PB_SettleCurrency}" EffectiveYield="" Commission="{CommissionCharged}" SECFee="{StampDuty}"
        OtherFees="{OtherBrokerFee}" AccruedInterest="{AccruedInterest}" NetTradeCash="{NetAmount}" Counterparty="{CounterParty}"
        TradeID="{TradeRefID}" Comments="{Description}" ClosingMethod="{$varClosingMethod}"
       
        SpecifiedLot1OpenDate1="{$varClosedAgainstDate1}" SpecifiedLot1Price1 ="{$varClosedPrice1}" SpecifiedLot1Qty1="{$varQ1}"
        SpecifiedLot1OpenDate2="{$varClosedAgainstDate2}" SpecifiedLot1Price2 ="{$varClosedPrice2}"  SpecifiedLot1Qty2="{$varQ2}" 
        SpecifiedLot1OpenDate3="{$varClosedAgainstDate3}"  SpecifiedLot1Price3 ="{$varClosedPrice3}" SpecifiedLot1Qty3="{$varQ3}" 
        SpecifiedLot1OpenDate4="{$varClosedAgainstDate4}" SpecifiedLot1Price4 ="{$varClosedPrice4}"  SpecifiedLot1Qty4="{$varQ4}" 
        SpecifiedLot1OpenDate5="{$varClosedAgainstDate5}" SpecifiedLot1Price5 ="{$varClosedPrice5}"  SpecifiedLot1Qty5="{$varQ5}" 
        SpecifiedLot1OpenDate6="{$varClosedAgainstDate6}" SpecifiedLot1Price6 ="{$varClosedPrice6}"  SpecifiedLot1Qty6="{$varQ6}" 
        SpecifiedLot1OpenDate7="{$varClosedAgainstDate7}" SpecifiedLot1Price7 ="{$varClosedPrice7}"  SpecifiedLot1Qty7="{$varQ7}" 
        SpecifiedLot1OpenDate8="{$varClosedAgainstDate8}"  SpecifiedLot1Price8 ="{$varClosedPrice8}" SpecifiedLot1Qty8="{$varQ8}" 
        SpecifiedLot1OpenDate9="{$varClosedAgainstDate9}" SpecifiedLot1Price9 ="{$varClosedPrice9}"  SpecifiedLot1Qty9="{$varQ9}" 
        SpecifiedLot1OpenDate10="{$varClosedAgainstDate10}" SpecifiedLot1Price10 ="{$varClosedPrice10}"  SpecifiedLot1Qty10="{$varQ10}"

        EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

    </Group>
  </xsl:template>
</xsl:stylesheet>
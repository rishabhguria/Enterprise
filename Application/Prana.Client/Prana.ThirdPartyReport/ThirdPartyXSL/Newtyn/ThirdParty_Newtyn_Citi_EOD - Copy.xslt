<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofZeros">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofZeros">
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

    <xsl:variable name="AllocatedQty" />
    <!-- Building a Group with the EntityID $I_TaxLotID... -->

    <!--Total Quantity-->
    <xsl:variable name="QtySum">
      <xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
    </xsl:variable>

    <xsl:variable name="QtySum1">
      <xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
    </xsl:variable>

    <!--Total Commission-->
    <xsl:variable name="VarCommissionSum">
      <xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/CommissionCharged)"/>
    </xsl:variable>
    <!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Symbol"/>
		</xsl:variable>-->
    <xsl:variable name="tempSideVar">
      <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Side"/>
    </xsl:variable>

    <!--Side-->

    <xsl:variable name="Sidevar">
      <xsl:choose>
        <xsl:when test="Side='Buy'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="Side='Sell'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="Side='Sell short'">
          <xsl:value-of select="'SS'"/>
        </xsl:when>
        <xsl:when test="Side='Buy to Close'">
          <xsl:value-of select="'BuyToClose'"/>
        </xsl:when>
        <xsl:when test="Side='Sell to Open'">
          <xsl:value-of select="'SellToOpen'"/>
        </xsl:when>
        <xsl:when test="Side='Sell to Close'">
          <xsl:value-of select="'SellToClose'"/>
        </xsl:when>
        <xsl:when test="Side='Buy to Open'">
          <xsl:value-of select="'BuyToOpen'"/>
        </xsl:when>
        <xsl:otherwise> </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="tempTaxlotStateVar">
      <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotStateID>1]/TaxLotState"/>
    </xsl:variable>

    <xsl:variable name="varTransactionType">
      <xsl:choose>
        <xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
          <xsl:value-of select="'BY'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell' or Side = 'Sell to Close'">
          <xsl:value-of select="'SL'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
          <xsl:value-of select="'SS'"/>
        </xsl:when>
        <xsl:when test="Side = 'Buy to Close'">
          <xsl:value-of select="'BC'"/>
        </xsl:when>
        <xsl:when test="Side = 'Buy'">
          <xsl:value-of select="'BY'"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varClientAccount">
      <xsl:choose>
        <xsl:when test ="AccountName = 'Citi Newtyn Partners LP:522-91K57'">
          <xsl:value-of select ="'52291K57'"/>
        </xsl:when>
        <xsl:when test ="AccountName = 'Citi Newtyn TE Partners LP:522-91K58'">
          <xsl:value-of select ="'52291K58'"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varInstruction">
      <xsl:choose>
        <xsl:when test="TaxLotState = 'Allocated'">
          <xsl:value-of select="'NEW'"/>
        </xsl:when>
        <xsl:when test="TaxLotState = 'Amended'">
          <xsl:value-of select="'MOD'"/>
        </xsl:when>
        <xsl:when test="TaxLotState = 'Deleted'">
          <xsl:value-of select="'CXL'"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

	  <xsl:variable name="varSpace">
		  <xsl:call-template name="noofBlanks">
			  <xsl:with-param name="count1" select="6-(string-length(UnderlyingSymbol))"/>
		  </xsl:call-template>
	  </xsl:variable>

	  <xsl:variable name="varUnderlying">
		  <xsl:value-of select="concat(UnderlyingSymbol, $varSpace)"/>
	  </xsl:variable>

	  <xsl:variable name="varIntStrike">
		  <xsl:choose>
			  <xsl:when test="contains(StrikePrice, '.')!= false">
				  <xsl:value-of select="substring-before(StrikePrice,'.')"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="StrikePrice"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varIntStrikeUpdated">
		  <xsl:call-template name="noofZeros">
			  <xsl:with-param name="count1" select="5-(string-length($varIntStrike))"/>
		  </xsl:call-template>
	  </xsl:variable>

	  <xsl:variable name="varDecimalStrike">
		  <xsl:choose>
			  <xsl:when test="contains(StrikePrice, '.')!= false">
				  <xsl:value-of select="substring-after(StrikePrice,'.')"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="0"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varDecimalStrikeUpdated">
		  <xsl:call-template name="noofZeros">
			  <xsl:with-param name="count1" select="3-(string-length($varDecimalStrike))"/>
		  </xsl:call-template>
	  </xsl:variable>

	  <xsl:variable name="varOSI">
		  <xsl:value-of select="concat($varUnderlying,' ', substring(ExpirationDate, 9, 2), substring(ExpirationDate, 1, 2), substring(ExpirationDate, 4, 2),substring(PutOrCall,1,1), $varIntStrikeUpdated, $varIntStrike, $varDecimalStrikeUpdated, $varDecimalStrike)"/>
	  </xsl:variable>
	  
    <xsl:variable name="varSecurityID">
      <xsl:choose>
        <xsl:when test="Asset = 'EquityOption' and OSIOptionSymbol != ''">
          <xsl:value-of select="$varOSI"/>
        </xsl:when>
        <!--<xsl:when test="ISIN != ''">
          <xsl:value-of select="ISIN"/>
        </xsl:when>
        <xsl:when test="CUSIP != ''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
        <xsl:when test="SEDOL != ''">
          <xsl:value-of select="SEDOL"/>
        </xsl:when>-->
        <xsl:otherwise>
          <xsl:value-of select="Symbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

	  <xsl:variable name="varSecurityType">
		  <xsl:choose>
			  <xsl:when test="Asset = 'EquityOption'">
				  <xsl:value-of select="'OCC'"/>
			  </xsl:when>
			  <!--<xsl:when test="ISIN != ''">
          <xsl:value-of select="ISIN"/>
        </xsl:when>
        <xsl:when test="CUSIP != ''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
        <xsl:when test="SEDOL != ''">
          <xsl:value-of select="SEDOL"/>
        </xsl:when>-->
			  <xsl:otherwise>
				  <xsl:value-of select="'TCKR'"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

    <xsl:variable name="varFeeType1">
      <xsl:choose>
        <xsl:when test="number(StampDuty)">
          <xsl:value-of select="'SEC'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeValue1">
      <xsl:choose>
        <xsl:when test="number(StampDuty)">
          <xsl:value-of select="StampDuty"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeType2">
      <xsl:choose>
        <xsl:when test="number(MiscFees)">
          <xsl:value-of select="'MSCF'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeValue2">
      <xsl:choose>
        <xsl:when test="number(MiscFees)">
          <xsl:value-of select="MiscFees"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>



    <xsl:variable name="varSymbol">
      <xsl:choose>
        <xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity' and CUSIP != ''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="Symbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

	  <xsl:variable name ="varClosingAlgo">
		  <xsl:choose>
			  <xsl:when test ="ClosingAlgo = 'HIFO'">
				  <xsl:value-of select ="''"/>
			  </xsl:when>
			  <xsl:when test= "ClosingAlgo != '*' and ClosingAlgo != ''">
				  <xsl:value-of select ="'V'"/>
			  </xsl:when>
		  </xsl:choose>
	  </xsl:variable>

    <!--Exchange Mapping-->

    <xsl:variable name="varExchangeName">
      <xsl:value-of select="'NASDAQ'"/>
    </xsl:variable>

	  <xsl:variable name ="varCount">
		  <xsl:choose>
			  <xsl:when test ="ClosingAlgo != '*' and ClosingAlgo != '' and ClosingAlgo != 'HIFO'">
				  <xsl:value-of select ="count(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID])"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select ="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
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
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varQ7">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=7]/ClosedQty"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varQ8">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=8]/ClosedQty"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varQ9">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=9]/ClosedQty"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varQ10">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=10]/ClosedQty"/>
		</xsl:if>    </xsl:variable>

	  <xsl:variable name="varQ11">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=11]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ12">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=12]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ13">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=13]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ14">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=14]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ15">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=15]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ16">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=16]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ17">
		  <xsl:if test ="ClosingAlgo = 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=17]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ18">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=18]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ19">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=19]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ20">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=20]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ21">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=21]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ22">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=22]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ23">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=23]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ24">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=24]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ25">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=25]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ26">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=26]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ27">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=27]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ28">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=28]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varQ29">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=29]/ClosedQty"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varQ30">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=30]/ClosedQty"/>
		  </xsl:if>	  </xsl:variable>


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
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedPrice4">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/OpenPriceAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedPrice5">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/OpenPriceAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedPrice6">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=6]/OpenPriceAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedPrice7">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=7]/OpenPriceAgainstClosing"/>
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="varClosedPrice8">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=8]/OpenPriceAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedPrice9">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=9]/OpenPriceAgainstClosing"/>
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="varClosedPrice10">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=10]/OpenPriceAgainstClosing"/>
		</xsl:if>    </xsl:variable>

	  <xsl:variable name="varClosedPrice11">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=11]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice12">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=12]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice13">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=13]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice14">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=14]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice15">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=15]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice16">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=16]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice17">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=17]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice18">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=18]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice19">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=19]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice20">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=20]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice21">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=21]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice22">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=22]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice23">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=23]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice24">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=24]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice25">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=25]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice26">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=26]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice27">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=27]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice28">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=28]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice29">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=29]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedPrice30">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=30]/OpenPriceAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>


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
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedAgainstDate4">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/TradeDateAgainstClosing"/>
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="varClosedAgainstDate5">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/TradeDateAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedAgainstDate6">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=6]/TradeDateAgainstClosing"/>
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="varClosedAgainstDate7">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=7]/TradeDateAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedAgainstDate8">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=8]/TradeDateAgainstClosing"/>
		</xsl:if>    </xsl:variable>

    <xsl:variable name="varClosedAgainstDate9">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=9]/TradeDateAgainstClosing"/>
		</xsl:if>
	</xsl:variable>

	<xsl:variable name="varClosedAgainstDate10">
		<xsl:if test ="ClosingAlgo != 'HIFO'">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=10]/TradeDateAgainstClosing"/>
		</xsl:if>    </xsl:variable>


	  <xsl:variable name="varClosedAgainstDate11">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=11]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate12">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=12]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate13">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=13]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate14">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=14]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate15">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=15]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate16">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=16]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate17">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=17]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate18">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=18]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate19">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=19]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate20">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=20]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>


	  <xsl:variable name="varClosedAgainstDate21">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=21]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate22">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=22]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate23">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=23]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate24">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=24]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate25">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=25]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate26">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=26]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate27">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=27]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate28">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=28]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate29">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=29]/TradeDateAgainstClosing"/>
		  </xsl:if>
	  </xsl:variable>

	  <xsl:variable name="varClosedAgainstDate30">
		  <xsl:if test ="ClosingAlgo != 'HIFO'">
			  <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=30]/TradeDateAgainstClosing"/>
		  </xsl:if>	  </xsl:variable>

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
	  
    <Group 
		RowHeader="FALSE"	RecordType ="Trade" TransactionType ="{$varTransactionType}" ClientAccount="{$varClientAccount}" AccountType=""
        ClientReference="{EntityID}" AmendNo="" Instruction="{$varInstruction}" BrokerCode="{CounterParty}"
        SecurityId="{$varSecurityID}" SecurityIdType="{$varSecurityType}" TradeDate = "{TradeDate}" SettlementDate = "{SettlementDate}"
        Quantity="{AllocatedQty}" Price="{$varAvgPrice}" TradeCurrency="{$PB_SettleCurrency}" CommissionCode="G"
        Commission="{CommissionCharged}" NetAmount="{NetAmount}" MemoField1="" MemoField2="" RepoRate=""
        RepoTerminationDate="" RepoEndMoney="" RepoAccruedInterest="" RepoHaircut="" SettlementLocation="" AccountNo=""
        AgentBankName="" AgentBankLocation="" AgentBankInstructions="" FeeType1="{$varFeeType1}" FeeValue1="{$varFeeValue1}"
        FeeType2="{$varFeeType2}" FeeValue2="{$varFeeValue2}" FeeType3="" FeeValue3="" Strategy="" TaxlotId=""
        PreFiguredIndicator="N" BondInterest="" BondPrincipal="" ProcessingType="" BlockId=""
        VSPMethod="{$varClosingAlgo}" Count="{$varCount}"
        VSPDate1="{$varClosedAgainstDate1}" VSPQuantity1="{$varQ1}" VSPPrice1="{$varClosedPrice1}"
        VSPDate2="{$varClosedAgainstDate2}" VSPQuantity2="{$varQ2}" VSPPrice2="{$varClosedPrice2}"
        VSPDate3="{$varClosedAgainstDate3}" VSPQuantity3="{$varQ3}" VSPPrice3="{$varClosedPrice3}"
        VSPDate4="{$varClosedAgainstDate4}" VSPQuantity4="{$varQ4}" VSPPrice4="{$varClosedPrice4}"
        VSPDate5="{$varClosedAgainstDate5}" VSPQuantity5="{$varQ5}" VSPPrice5="{$varClosedPrice5}"
        VSPDate6="{$varClosedAgainstDate6}" VSPQuantity6="{$varQ6}" VSPPrice6="{$varClosedPrice6}"
        VSPDate7="{$varClosedAgainstDate7}" VSPQuantity7="{$varQ7}" VSPPrice7="{$varClosedPrice7}"
        VSPDate8="{$varClosedAgainstDate8}" VSPQuantity8="{$varQ8}" VSPPrice8="{$varClosedPrice8}"
        VSPDate9="{$varClosedAgainstDate9}" VSPQuantity9="{$varQ9}" VSPPrice9="{$varClosedPrice9}"
        VSPDate10="{$varClosedAgainstDate10}" VSPQuantity10="{$varQ10}" VSPPrice10="{$varClosedPrice10}"
		
		VSPDate11="{$varClosedAgainstDate11}" VSPQuantity11="{$varQ11}" VSPPrice11="{$varClosedPrice11}"
        VSPDate12="{$varClosedAgainstDate12}" VSPQuantity12="{$varQ12}" VSPPrice12="{$varClosedPrice12}"
        VSPDate13="{$varClosedAgainstDate13}" VSPQuantity13="{$varQ13}" VSPPrice13="{$varClosedPrice13}"
        VSPDate14="{$varClosedAgainstDate14}" VSPQuantity14="{$varQ14}" VSPPrice14="{$varClosedPrice14}"
        VSPDate15="{$varClosedAgainstDate15}" VSPQuantity15="{$varQ15}" VSPPrice15="{$varClosedPrice15}"
        VSPDate16="{$varClosedAgainstDate16}" VSPQuantity16="{$varQ16}" VSPPrice16="{$varClosedPrice16}"
        VSPDate17="{$varClosedAgainstDate17}" VSPQuantity17="{$varQ17}" VSPPrice17="{$varClosedPrice17}"
        VSPDate18="{$varClosedAgainstDate18}" VSPQuantity18="{$varQ18}" VSPPrice18="{$varClosedPrice18}"
        VSPDate19="{$varClosedAgainstDate19}" VSPQuantity19="{$varQ19}" VSPPrice19="{$varClosedPrice19}"
        VSPDate20="{$varClosedAgainstDate20}" VSPQuantity20="{$varQ20}" VSPPrice20="{$varClosedPrice20}"
		
		VSPDate21="{$varClosedAgainstDate21}" VSPQuantity21="{$varQ21}" VSPPrice21="{$varClosedPrice21}"
        VSPDate22="{$varClosedAgainstDate22}" VSPQuantity22="{$varQ22}" VSPPrice22="{$varClosedPrice22}"
        VSPDate23="{$varClosedAgainstDate23}" VSPQuantity23="{$varQ23}" VSPPrice23="{$varClosedPrice23}"
        VSPDate24="{$varClosedAgainstDate24}" VSPQuantity24="{$varQ24}" VSPPrice24="{$varClosedPrice24}"
        VSPDate25="{$varClosedAgainstDate25}" VSPQuantity25="{$varQ25}" VSPPrice25="{$varClosedPrice25}"
        VSPDate26="{$varClosedAgainstDate26}" VSPQuantity26="{$varQ26}" VSPPrice26="{$varClosedPrice26}"
        VSPDate27="{$varClosedAgainstDate27}" VSPQuantity27="{$varQ27}" VSPPrice27="{$varClosedPrice27}"
        VSPDate28="{$varClosedAgainstDate28}" VSPQuantity28="{$varQ28}" VSPPrice28="{$varClosedPrice28}"
        VSPDate29="{$varClosedAgainstDate29}" VSPQuantity29="{$varQ29}" VSPPrice29="{$varClosedPrice29}"
        VSPDate30="{$varClosedAgainstDate30}" VSPQuantity30="{$varQ30}" VSPPrice30="{$varClosedPrice30}"

        EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

    </Group>
  </xsl:template>
</xsl:stylesheet>

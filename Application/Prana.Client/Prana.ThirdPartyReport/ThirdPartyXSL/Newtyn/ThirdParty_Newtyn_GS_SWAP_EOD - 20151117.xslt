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
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
          <!-- ...buid a Group for this node_id -->
          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_GroupID">
              <xsl:value-of select="PBUniqueID" />
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
      </xsl:for-each>
    </Groups>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_GroupID" />

    <xsl:variable name="AllocatedQty" />
    <!-- Building a Group with the EntityID $I_TaxLotID... -->

    <!--Total Quantity-->
    <!--<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_GroupID][TaxLotState != 'Deleted']/ClosedQty)"/>
		</xsl:variable>

		<xsl:variable name="QtySum1">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_GroupID][TaxLotState != 'Deleted']/ClosedQty)"/>
		</xsl:variable>

		-->
    <!--Total Commission-->
    <!--
		<xsl:variable name="VarCommissionSum">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		-->
    <!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Symbol"/>
		</xsl:variable>-->
    <!--
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_GroupID]/Side"/>
		</xsl:variable>-->



    <xsl:variable name="Quantity">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
    </xsl:variable>
    <xsl:variable name="CommissionSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
    </xsl:variable>
    <xsl:variable name="TaxOnCommissionSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
    </xsl:variable>
    <xsl:variable name="StampDutySum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/StampDuty)"/>
    </xsl:variable>
    <xsl:variable name="TransactionLevySum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TransactionLevy)"/>
    </xsl:variable>
    <xsl:variable name="ClearingFeeSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingFee)"/>
    </xsl:variable>
    <xsl:variable name="MiscFeesSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/MiscFees)"/>
    </xsl:variable>
    <xsl:variable name="OtherBrokerFeeSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OtherBrokerFee)"/>
    </xsl:variable>
    <xsl:variable name="GrossAmountSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/GrossAmount)"/>
    </xsl:variable>
    <xsl:variable name="NetAmountSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
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
    <!--<xsl:variable name="tempTaxlotStateVar">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>-->

    <xsl:variable name="varTransactionType">
      <xsl:choose>
        <xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell' or Side = 'Sell to Close'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell short' or Side = 'Sell to Open'">
          <xsl:value-of select="'SS'"/>
        </xsl:when>
        <xsl:when test="Side = 'Buy to Close' or Side = 'Buy to Cover'">
          <xsl:value-of select="'BC'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varUnderlyingSymbol">
      <xsl:choose>
        <xsl:when test ="contains(Asset, 'Option')!= false">
          <xsl:value-of select ="UnderlyingSymbol"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varStrikePrice">
      <xsl:choose>
        <xsl:when test ="contains(Asset, 'Option')!= false">
          <xsl:value-of select ="StrikePrice"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>



    <xsl:variable name="varSymbol">
      <xsl:choose>
        <xsl:when test="SEDOL != '' and SEDOL != '*'">
          <xsl:value-of select="SEDOL"/>
        </xsl:when>
        <xsl:when test="ISIN != '*' and ISIN != ''">
          <xsl:value-of select="ISIN"/>
        </xsl:when>
        <xsl:when test="CUSIP != '*' and CUSIP != ''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
       
        <xsl:when test="BBCode != '*' and BBCode != ''">
          <xsl:value-of select="BBCode"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="Symbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>


    <!--<xsl:variable name ="varLotID1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varLotID5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="varQ2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varClosedPrice1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varClosedAgainstDate1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

    <!--<xsl:variable name ="UDA_Country">
      <xsl:value-of select ="UDACountryName"/>
    </xsl:variable>
    <xsl:variable name ="PB_SettleCurrency">
      <xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name='ALL']/SymbolData[@UDACountry=$UDA_Country]/@SettleCurrency"/>
    </xsl:variable>-->

    <xsl:variable name="TradeTax" select="$TransactionLevySum + $StampDutySum"/>

    <xsl:variable name ="varTradeTax">
      <xsl:choose>
        <xsl:when test="$TradeTax &gt; 0">
          <xsl:value-of select="$TradeTax"/>
        </xsl:when>
        <xsl:when test="$TradeTax &lt; 0">
          <xsl:value-of select="$TradeTax * (-1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="0"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="MiscMoney">
      <xsl:value-of select="$OtherBrokerFeeSum + $ClearingFeeSum + $TaxOnCommissionSum + $MiscFeesSum"/>
    </xsl:variable>

    <xsl:variable name="varMiscMoney">
      <xsl:choose>
        <xsl:when test="$MiscMoney &gt; 0">
          <xsl:value-of select="$MiscMoney"/>
        </xsl:when>
        <xsl:when test="$MiscMoney &lt; 0">
          <xsl:value-of select="$MiscMoney * (-1)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="0"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varAvgPrice">
      <!--<xsl:choose>
        <xsl:when test ="CounterParty = 'GWEP'">
          <xsl:value-of  select="format-number(AveragePrice,'#.0000')"/>
        </xsl:when>
        <xsl:otherwise>-->
          <xsl:value-of  select="AveragePrice"/>
        <!--</xsl:otherwise>
      </xsl:choose>-->
    </xsl:variable>

    <xsl:variable name="SettleFx">
      <xsl:choose>
        <xsl:when test="number(SettlCurrFxRate)">
          <xsl:value-of select="SettlCurrFxRate"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="1"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="AvgPriceFX">
      <xsl:choose>
        <xsl:when test="SettlCurrFxRateCalc='M'">
          <xsl:value-of select="$varAvgPrice * $SettleFx"/>
        </xsl:when>

        <xsl:when test="SettlCurrFxRateCalc='D'">
          <xsl:value-of select="$varAvgPrice div $SettleFx"/>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$varAvgPrice"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varAllocationState">
      <xsl:choose>
        <xsl:when test ="TaxLotState = 'Allocated'">
          <xsl:value-of  select="'N'"/>
        </xsl:when>
        <xsl:when test ="TaxLotState = 'Amended'">
          <xsl:value-of  select="'A'"/>
        </xsl:when>
        <xsl:when test ="TaxLotState = 'Deleted'">
          <xsl:value-of  select="'C'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of  select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varSecurityName">
      <!--<xsl:choose>
        <xsl:when test="Asset='Equity' and IsSwapped='true'">
          <xsl:value-of select="'SWAP'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>-->

      <xsl:choose>
        <xsl:when test="Asset='Equity' and IsSwapped='true'">
          <xsl:value-of select="'CFD'"/>
        </xsl:when>
        <xsl:when test="SEDOL != '' and SEDOL != '*'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="ISIN != '*' and ISIN != ''">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="CUSIP != '*' and CUSIP != ''">
          <xsl:value-of select="'C'"/>
        </xsl:when>

        <xsl:when test="BBCode != '*' and BBCode != ''">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="GrossAmount">
      <xsl:value-of select="$GrossAmountSum"/>
    </xsl:variable>

    <xsl:variable name="GrossAmountFX">
      <xsl:choose>
        <xsl:when test="SettlCurrFxRateCalc='M'">
          <xsl:value-of select="$GrossAmount * $SettleFx"/>
        </xsl:when>

        <xsl:when test="SettlCurrFxRateCalc='D'">
          <xsl:value-of select="$GrossAmount div $SettleFx"/>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$GrossAmount"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="PB_NAME" select="'GS'"/>

    <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

    <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
      <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
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

    <xsl:variable name="NetAmountFX">
      <xsl:choose>
        <xsl:when test="SettlCurrFxRateCalc='M'">
          <xsl:value-of select="$NetAmountSum * $SettleFx"/>
        </xsl:when>

        <xsl:when test="SettlCurrFxRateCalc='D'">
          <xsl:value-of select="$NetAmountSum div $SettleFx"/>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$NetAmountSum"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="ExpiryDate">
      <xsl:choose>
        <xsl:when test="Asset='EquityOption'">
          <xsl:value-of select="concat(substring(substring-after(substring-after(ExpirationDate,'/'),'/'),3,2),substring-before(ExpirationDate,'/'),substring-after(substring-before(ExpirationDate,'/'),'/'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="CommissionFX">

      <xsl:choose>
        <xsl:when test="SettlCurrFxRateCalc='M'">
          <xsl:value-of select="($CommissionSum) * $SettleFx"/>
        </xsl:when>

        <xsl:when test="SettlCurrFxRateCalc='D'">
          <xsl:value-of select="($CommissionSum) div $SettleFx"/>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="($CommissionSum)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!--<xsl:variable name="Quantity">
			<xsl:choose>
				<xsl:when test="contains(Asset,'Future')">
					<xsl:value-of select="AllocatedQty * AssetMultiplier"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="AllocatedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

    <xsl:variable name="CurrencyCode">
      <xsl:choose>
        <xsl:when test="contains(Asset,'FX')">
          <xsl:value-of select="VsCurrencyName"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="SettlCurrency"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name = "PRANA_FUND_NAME">
      <xsl:value-of select="FundName"/>
    </xsl:variable>

    <xsl:variable name ="THIRDPARTY_FUND_CODE">
      <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
    </xsl:variable>


    <xsl:variable name="AccountName">
      <xsl:choose>
        <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
          <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$PRANA_FUND_NAME"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
  

    <Group
				OrderNumber ="{EntityID}" Cancelcorrectindicator ="{$varAllocationState}" AccountNumber="{$AccountName}" SecurityIdentifier="{$varSymbol}"

				Broker="{CounterParty}" Custodian="GSCO" TransactionType="{$varTransactionType}" CurrencyCode="{$CurrencyCode}"
				TradeDate = "{TradeDate}" SettleDate = "{SettlementDate}"
				Quantity="{$Quantity}" Commission="{$CommissionSum}" Price="{AveragePrice}" AccruedInterest="{AccruedInterest}"
				TradeTax="{$varTradeTax}" MiscMoney="{$varMiscMoney}" NetAmount="{$NetAmountSum}" Principal="{$GrossAmountSum}" Description="{FullSecurityName}"
				SecurityType="{$varSecurityName}" CountrySettlementCode="" ClearingAgent="" SECFee="{StampDuty}" RepoOpenSettleDate="" RepoMaturityDate=""
		  RepoRate="" RepoInterest="" OptionUnderlyer="{$varUnderlyingSymbol}" OptionExpiryDate="{$ExpiryDate}" OptionCallPutIndicator="{substring(PutOrCall,1,1)}"
		  OptionStrikePrice="{$varStrikePrice}" Trailer="TEXT"
		  GenevaLotNumber1="" GainsKeeperLotNumber1="" LotDate1="" LotQty1="" LotPrice1=""
		  GenevaLotNumber2 ="" GainsKeeperLotNumber2="" LotDate2="" LotQty2="" LotPrice2=""
		  GenevaLotNumber3="" GainsKeeperLotNumber3="" LotDate3="" LotQty3="" LotPrice3=""
		  GenevaLotNumber4 ="" GainsKeeperLotNumber4="" LotDate4="" LotQty4="" LotPrice4=""
		  GenevaLotNumber5="" GainsKeeperLotNumber5="" LotDate5="" LotQty5="" LotPrice5=""
       	  EntityID="{EntityID}" TaxLotState="{TaxLotState}">

    </Group>
  </xsl:template>
</xsl:stylesheet>
<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
  <xsl:template match="/">
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
		
		<Group
			 TranType ="Tran Type" Quantity="Quantity" BBGTicker="BBG Ticker" ProductCode="Product Code" SecurityType="Security Type"
						MaturityMonth="Maturity Month" MaturityYear="Maturity Year" Exchange="Exchange" SecurityDescription="Security Description" PricePremium="Price/Premium"
						PutorCall="Put or Call" StrikePrice="Strike Price" ExecutingBroker="Executing Broker" TradeDate="Trade Date" HoldingAccount="Holding Account" FCMAccount="FCM Account" FCM="FCM" AllocType="AllocType" TradeID="TradeID"           
						EntityID="" TaxLotState="" RowHeader="false"/>
      
		  <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[Asset='Future']">
			  <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
			  <!--<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">-->
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
		<!-- Building a Group with the EntityID $I_GroupID... -->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AllocatedQty)"/>
		</xsl:variable>
		<xsl:variable name="GroupNetAmt">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/NetAmount)"/>
		</xsl:variable>
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="BrokerCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OtherBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="TaxOnCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxOnCommissions)"/>
		</xsl:variable>
		<xsl:variable name="SecFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SecFee)"/>
		</xsl:variable>
		<xsl:variable name="StampDutySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/StampDuty)"/>
		</xsl:variable>
		<xsl:variable name="TransactionLevySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TransactionLevy)"/>
		</xsl:variable>
		<xsl:variable name="ClearingFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/ClearingFee)"/>
		</xsl:variable>
		<xsl:variable name="MiscFeesSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/MiscFees)"/>
		</xsl:variable>
		<xsl:variable name="OrfFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OrfFee)"/>
		</xsl:variable>
		<xsl:variable name="OtherBrokerFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OtherBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="GrossAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/GrossAmount)"/>
		</xsl:variable>
		<!--<xsl:variable name="NetAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
		</xsl:variable>-->
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>

		<xsl:variable name="tempCPVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CounterParty"/>
		</xsl:variable>

		<xsl:variable name="PB_NAME" select="'MS'"/>

		<xsl:variable name="PB_COUNTERPARTY_NAME">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$tempCPVar]/@ThirdPartyBrokerID"/>
		</xsl:variable>

		<xsl:variable name="CPVar">
			<!--<xsl:choose>
				<xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
				<xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
				<xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>-->
			<xsl:choose>
				<xsl:when test="$PB_COUNTERPARTY_NAME!=''">
					<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="Sidevar">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">BUY</xsl:when>
				<xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">BUY TO CLOSE</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">SELL</xsl:when>
				<xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">SELL SHORT</xsl:when>
				<xsl:otherwise> </xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tempTaxlotStateVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>

		<xsl:variable name="varTaxlotStateGrp">
			<xsl:choose>
				<xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
				<xsl:otherwise>NEW</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


    <xsl:variable name="varProductCode">
      <xsl:value-of select ="substring(BBCode,1,2)"/>
    </xsl:variable>
    
    <xsl:variable name="varSecurityType">
      <xsl:choose>
        <xsl:when test="Asset='Future'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMaturityMonth">
      <xsl:value-of select ="substring-before(ExpirationDate,'/')"/>
    </xsl:variable>
    
    <xsl:variable name="varMaturityYear">
      <xsl:value-of select ="substring-after(substring-after(ExpirationDate,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name="varAccount">
      <xsl:value-of select ="concat(ABC,PBUniqueID)"/>
    </xsl:variable>

		<xsl:variable name="varAccount1">
			<xsl:value-of select ="concat($varAccount,'01')"/>
		</xsl:variable>

	  <xsl:variable name="varAveragePrice">
		  <xsl:value-of select ="format-number(AveragePrice,'#.####')"/>
	  </xsl:variable>

	  <xsl:variable name="varAllocatedQty">
		  <xsl:choose>
			  <xsl:when test="Side='Sell' or Side='Sell Short'">
				  <xsl:value-of select="AllocatedQty *-1"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select ="AllocatedQty"/>
			  </xsl:otherwise>
		  </xsl:choose>		  
	  </xsl:variable>

	  <xsl:variable name="i" select="position()" />


	  <xsl:variable name="varFCMAccount">
		  <xsl:choose>
			  <xsl:when test="$i &lt; 10">
				  <xsl:value-of select="concat($I_GroupID,'0',$i)"/>
			  </xsl:when>

			  <xsl:otherwise>
				  <xsl:value-of select="concat($I_GroupID,$i)"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

		<xsl:variable name="varBBGTicker">
			<xsl:value-of select ="substring-before(BBCode,' ')"/>
		</xsl:variable>

    <Group
        
					  TranType ="{$Sidevar}" Quantity="{$varAllocatedQty}" BBGTicker="{$varBBGTicker}" ProductCode="{$varProductCode}" SecurityType="{$varSecurityType}"
						MaturityMonth="{$varMaturityMonth}" MaturityYear="{$varMaturityYear}" Exchange="{Exchange}" SecurityDescription="{FullSecurityName}" PricePremium="{$varAveragePrice}"
						PutorCall="" StrikePrice="" ExecutingBroker="{CounterParty}" TradeDate="{TradeDate}" HoldingAccount="{$varAccount1}" FCMAccount="{$varAccount}" FCM="GU FCM" AllocType="AVG" TradeID="{EntityID}"           
						EntityID="{EntityID}" TaxLotState="{TaxLotState}">

      <!-- ...selecting all the records for this Group... -->
		<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
			<!-- ...and building a ThirdPartyFlatFileDetail for each -->
        <xsl:variable name="taxLotIDVar" select="EntityID"/>

        <xsl:variable name="varTaxlotState">
          <xsl:choose>
            <xsl:when test="TaxLotState='Allocated'">
              <xsl:value-of select ="'NEW'"/>
            </xsl:when>
            <xsl:when test="TaxLotState='Amended'">
              <xsl:value-of select ="'COR'"/>
            </xsl:when>
            <xsl:when test="TaxLotState='Deleted'">
              <xsl:value-of select ="'CAN'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select ="''"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        


        <xsl:variable name="amp">N</xsl:variable>

        <ThirdPartyFlatFileDetail
							TaxLotState="{TaxLotState}" TranType ="{$Sidevar}" Quantity="{$varAllocatedQty}" BBGTicker="{$varBBGTicker}" ProductCode="{$varProductCode}" SecurityType="{$varSecurityType}"
						  MaturityMonth="{$varMaturityMonth}" MaturityYear="{$varMaturityYear}" Exchange="{Exchange}" SecurityDescription="{FullSecurityName}" PricePremium="{$varAveragePrice}"
						  PutorCall="" StrikePrice="" ExecutingBroker="{CounterParty}" TradeDate="{TradeDate}" HoldingAccount="{AccountNo}" FCMAccount="{$varAccount1}" FCM="GU FCM" AllocType="FILL" TradeID="{EntityID}"
							EntityID="{EntityID}" />
      </xsl:for-each>
    </Group>
  </xsl:template>
</xsl:stylesheet>
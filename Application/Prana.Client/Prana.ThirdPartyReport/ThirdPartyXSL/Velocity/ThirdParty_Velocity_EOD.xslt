<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
					<xsl:if test="CounterParty != 'CorpAction' and CounterParty != 'Transfer' and CounterParty != 'Transfer1' and CounterParty != 'WashSales'">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						<xsl:with-param name="I_GroupID">
							<xsl:value-of select="PBUniqueID" />
						</xsl:with-param></xsl:call-template>
				</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>

	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />
		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_GroupID... -->
		<xsl:variable name="Qty">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AllocatedQty"/>
		</xsl:variable>
		<xsl:variable name="Commission">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CommissionCharged"/>
		</xsl:variable>
		<xsl:variable name="SoftCommission">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SoftCommissionCharged"/>
		</xsl:variable>
		
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>

    <xsl:variable name="varCurrency">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CurrencySymbol"/>
    </xsl:variable>

    <xsl:variable name="varSEDOL">
      <xsl:value-of  select="concat(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SEDOL,'.')"/>
    </xsl:variable>

    <xsl:variable name="varTradedate">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TradeDate"/>
    </xsl:variable>

    <xsl:variable name="varSettlementDate">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SettlementDate"/>
    </xsl:variable>

		<xsl:variable name="Sidevar">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">B</xsl:when>
				<xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">BC</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">S</xsl:when>
				<xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">SS</xsl:when>
				<xsl:otherwise> </xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="PB_NAME" select="''"/>
		
		<xsl:variable name = "PRANA_FUND_NAME">
					<xsl:value-of select="AccountNo"/>
				</xsl:variable>
		
		<xsl:variable name ="varGroupEection_Mapping">
					<xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@ExecutionNO"/>
				</xsl:variable>

				<xsl:variable name="varEection">
					<xsl:choose>
						<xsl:when test="$varGroupEection_Mapping!=''">
							<xsl:value-of select="$varGroupEection_Mapping"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PRANA_FUND_NAME"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
    
    <xsl:variable name ="varTradeYear">
    <xsl:value-of select="substring-after(substring-after($varTradedate,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name ="varTradeMonth">
      <xsl:value-of select="substring-before($varTradedate,'/')"/>
    </xsl:variable>

    <xsl:variable name ="varTradeDay">
      <xsl:value-of select="substring-before(substring-after($varTradedate,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name ="varSettleYear">
      <xsl:value-of select="substring-after(substring-after($varSettlementDate,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name ="varSettleMonth">
      <xsl:value-of select="substring-before($varSettlementDate,'/')"/>
    </xsl:variable>

    <xsl:variable name ="varSettleDay">
      <xsl:value-of select="substring-before(substring-after($varSettlementDate,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name="varPershare">
      <xsl:value-of select="Commission div Qty"/>
    </xsl:variable>

    <xsl:variable name="varCommission" select="(Commission)+(SoftCommission)"/>
    <xsl:variable name ="varComm">
      <xsl:choose>
        <xsl:when test="Commission &lt;1">
          <xsl:value-of select="format-number($varPershare,'##.##')"/>
        </xsl:when>
        <xsl:when test="CommissionCharged &gt;1">
          <xsl:value-of select="concat('c',$varCommission)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

		<Group
				ACCOUNT = "{$varEection}" SIDE="{$Sidevar}" QUANTITY="{$Qty}" SEDOL="{$varSEDOL}" PRICE="{AveragePrice}"
				COMMISSION="{$varComm}" BROKERCODE="039131933" TRADEDATEYEAR="{$varTradeYear}" TRADEDATEMONTH="{$varTradeMonth}" TRADEDATEDAY="{$varTradeDay}"
				SETTLEDATEYEAR="{$varSettleYear}" SETTLEDATEMONTH="{$varSettleMonth}" SETTLEDATEDAY="{$varSettleDay}" MEMOFIELD="" CURRENCYCODE="{$varCurrency}" 
				EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="">


			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">			
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

		
				<xsl:variable name = "TaxlotPRANA_FUND_NAME">
					<xsl:value-of select="AccountNo"/>
				</xsl:variable>

				<xsl:variable name ="varExecution_Mapping">
					<xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$TaxlotPRANA_FUND_NAME]/@AccountNO"/>
				</xsl:variable>

				<xsl:variable name="varAccountNO">
					<xsl:choose>
						<xsl:when test="$varExecution_Mapping!=''">
							<xsl:value-of select="$varExecution_Mapping"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$TaxlotPRANA_FUND_NAME"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

        <xsl:variable name="TaxlotSidevar">
          <xsl:choose>
            <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">S</xsl:when>
            <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">SS</xsl:when>
            <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">B</xsl:when>
            <xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">BC</xsl:when>
            <xsl:otherwise> </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

				<ThirdPartyFlatFileDetail		
					
							Group_Id="" ACCOUNT = "{$varEection}" SIDE="{$TaxlotSidevar}" QUANTITY="{$Qty}" SEDOL="{$varSEDOL}" PRICE="{AveragePrice}"
				COMMISSION="{$varComm}" BROKERCODE="039131933" TRADEDATEYEAR="{$varTradeYear}" TRADEDATEMONTH="{$varTradeMonth}" TRADEDATEDAY="{$varTradeDay}"
				SETTLEDATEYEAR="{$varSettleYear}" SETTLEDATEMONTH="{$varSettleMonth}" SETTLEDATEDAY="{$varSettleDay}" MEMOFIELD="" CURRENCYCODE="{$varCurrency}"  EntityID="{EntityID}" TaxLotState="{TaxLotState}" />
			
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>

<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

	<xsl:key name="GroupID" match="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail/ParentSiblingInfo/parentSiblingInfo/parent" use="groupId"/>

	<xsl:key name="clorderid" match="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail/ParentSiblingInfo/parentSiblingInfo/parent/siblings/tradedOrder" use="CLOrderID"/>

  <xsl:key name="TaxlotId" match="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail" use="EntityID"/>
  <xsl:key name="FilExecId" match="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail/ParentSiblingInfo/parentSiblingInfo/parent/siblings/tradedOrder/fills/fill" use="ExecutionID" />

	<xsl:template match="/">

		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->

			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotFIXState!='Deleted' and TaxLotFIXState!='Amemded']">
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
		
		<!-- Building a Group with the EntityID $I_GroupID... -->
		
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TotalQty)"/>
		</xsl:variable>

		<xsl:variable name="AllocQtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>

    <xsl:variable name="AllocOrderCount">
      <xsl:value-of  select="count(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ParentSiblingInfo/parentSiblingInfo/parent/siblings/tradedOrder[generate-id(.)=generate-id(key('clorderid', CLOrderID)[1])])"/>
    </xsl:variable>
    <xsl:variable name="FillCount">
      <xsl:value-of  select="count(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ParentSiblingInfo/parentSiblingInfo/parent/siblings/tradedOrder/fills/fill[generate-id(.)=generate-id(key('FilExecId', ExecutionID)[1])])"/>
    </xsl:variable>

    <xsl:variable name ="FillsCount">
			<xsl:choose>
				<xsl:when test ="number($FillCount)">
					<xsl:value-of select="$FillCount"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

    <xsl:variable name="AllocTaxCount">
      <xsl:value-of  select="count(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][generate-id(.)=generate-id(key('TaxlotId', EntityID)[1])])"/>
    </xsl:variable>
		
		<!--<xsl:variable name="GroupNetAmt">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
		</xsl:variable>
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="TaxOnCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
		</xsl:variable>
		<xsl:variable name="SecFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SecFee)"/>
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
		<xsl:variable name="OrfFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OrfFee)"/>
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
		<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>
    <xsl:variable name="tempSideTagVar">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SideTag"/>
    </xsl:variable>

		<!--<xsl:variable name="tempCPVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CounterParty"/>
		</xsl:variable>

		<xsl:variable name="CPVar">
			<xsl:choose>
				<xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
				<xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
				<xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="Sidevar">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">BL</xsl:when>
				<xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">BC</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">SL</xsl:when>
				<xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">SS</xsl:when>
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
		</xsl:variable>-->


		<!--<xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
					<xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
					<xsl:value-of select="concat($varSymbolBef,'/',$varSymbolAft)"/>-->

		<!--<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					
					<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>					
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

		<!--<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>
				<xsl:when test="SEDOL != ''">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="ISIN != ''">
					<xsl:value-of select="ISIN"/>
				</xsl:when>
				<xsl:when test="Symbol != ''">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varSecType">
			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="SEDOL != ''">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="ISIN != ''">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="Symbol != ''">
					<xsl:value-of select="'T'"/>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select ="'T'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupGrsAmt">
			<xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
		</xsl:variable>

		<xsl:variable name="GroupSideMultiplier">
			<xsl:choose>
				<xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open' or $tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">
					<xsl:value-of select="1"/>
				</xsl:when>
				<xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close' or $tempSideVar='Sell short' or $tempSideVar='Sell to Open'">
					<xsl:value-of select="-1"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="GroupNetAmt1">
			<xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
		</xsl:variable>-->
    
    <xsl:variable name="TradeDay">
    <xsl:choose>
    <xsl:when test="string-length(substring-before(substring-after(TradeDate,'/'),'/'))=1">
      <xsl:value-of select="concat('0',substring-before(substring-after(TradeDate,'/'),'/'))"/>
    </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring-before(substring-after(TradeDate,'/'),'/')"/>
    </xsl:otherwise>
    </xsl:choose>
    </xsl:variable>
    
    <xsl:variable name="TradeMonth">
    <xsl:choose>
    <xsl:when test="string-length(substring-before(TradeDate,'/'))=1">
      <xsl:value-of select="concat('0',substring-before(TradeDate,'/'))"/>
    </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring-before(TradeDate,'/')"/>
    </xsl:otherwise>
    </xsl:choose>
    </xsl:variable>
    
    <xsl:variable name="TradeDate" select="concat(substring-after(substring-after(substring-before(TradeDate,' '),'/'),'/'),$TradeMonth,
                  $TradeDay)"/>
    
      <xsl:variable name="SettleDay">
    <xsl:choose>
    <xsl:when test="string-length(substring-before(substring-after(SettlementDate,'/'),'/'))=1">
      <xsl:value-of select="concat('0',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
    </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
    </xsl:otherwise>
    </xsl:choose>
    </xsl:variable>
    
    <xsl:variable name="SettleMonth">
    <xsl:choose>
    <xsl:when test="string-length(substring-before(SettlementDate,'/'))=1">
      <xsl:value-of select="concat('0',substring-before(SettlementDate,'/'))"/>
    </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="substring-before(SettlementDate,'/')"/>
    </xsl:otherwise>
    </xsl:choose>
    </xsl:variable>
    
    <xsl:variable name="SettleDate" select="concat(substring-after(substring-after(substring-before(SettlementDate,' '),'/'),'/'),$SettleMonth,
                  $SettleDay)"/>

		<xsl:variable name="SecurityType">
			<xsl:choose>
				<xsl:when test="Asset='Equity'">
					<xsl:value-of select="'CS'"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'Option')">
					<xsl:value-of select="'OPT'"/>
				</xsl:when>
				<xsl:when test="Asset='Future'">
					<xsl:value-of select="'FUT'"/>
				</xsl:when>
				<xsl:when test="Asset='ConvertibleBond'">
					<xsl:value-of select="'CORP'"/>
				</xsl:when>
				<xsl:when test="contains(Asset,'FX')">
					<xsl:value-of select="'FOR'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="MaturityMonth">
			<xsl:choose>
				<xsl:when test="string-length(substring-before(ExpirationDate,'/'))=1">
					<xsl:value-of select="concat('0',substring-before(ExpirationDate,'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(ExpirationDate,'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="MaturityMonthYear">
			<xsl:choose>
				<xsl:when test="contains(Asset,'Option') or Asset='Future'">
          <xsl:value-of select="concat(substring-after(substring-after(substring-before(ExpirationDate,' '),'/'),'/'),$MaturityMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="MaturityDay">
			<xsl:choose>
				<xsl:when test="contains(Asset,'Option') or Asset='Future'">
					<xsl:choose>
						<xsl:when test="string-length(substring-before(substring-after(ExpirationDate,'/'),'/'))=1">
							<xsl:value-of select="concat('0',substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="PutOrCall">
			<xsl:choose>
				<xsl:when test="PutOrCall='Put'">
					<xsl:value-of select="'0'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'1'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="StrikePrice">
			<xsl:choose>
				<xsl:when test="number(StrikePrice)">
					<xsl:value-of select="StrikePrice"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<Group GroupID="{/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]/ParentSiblingInfo/parentSiblingInfo/parent/groupId}" 
			AllocID="{/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]/ParentSiblingInfo/parentSiblingInfo/parent/groupId}" 
			TotalShares="{TotalQty}" AllocTransType="0" TradeDate="{$TradeDate}" FutSettDate="{$SettleDate}"
			Shares="{$AllocQtySum}" Symbol="{Symbol}" SecurityID="{BBCode}" IDSource="A" SecurityType="{$SecurityType}" MaturityMonthYear="{$MaturityMonthYear}" MaturityDay="{$MaturityDay}" PutOrCall="{$PutOrCall}"
			StrikePrice="{$StrikePrice}" AvgPrice="{AveragePrice}" OrderSideTagValue="{$tempSideTagVar}" NoOrders="{$AllocOrderCount}" NoAllocs="{$AllocTaxCount}" SettlCurrency="{SettlCurrency}"
			NoExecs="{$FillsCount}" OrderSide="{$tempSideVar}" Side="{$tempSideTagVar}" GroupFIXState="{TaxLotFIXState}" GroupFIXStateID="{TaxLotFIXStateID}" 
		GroupFIXAckState="{TaxLotFIXAckState}" GroupFIXAckStateID="{TaxLotFIXAckStateID}" GroupFIXStateCombined="{TaxLotFIXState}-{TaxLotFIXAckState}">

			<!-- ...selecting all the records for this Group... -->

			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]/ParentSiblingInfo/parentSiblingInfo/parent[generate-id(.)=generate-id(key('GroupID', groupId)[1])]">

				<!-- ...selecting all the Taxlots for this Group... -->

				<Taxlots>
					<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">

						<Taxlot TaxLotId="{EntityID}" AllocPrice="{AveragePrice}" 
              AllocAccount="{FundAccountNo}" AllocShares="{AllocatedQty}"/>

					</xsl:for-each>
				</Taxlots>

				<!-- ...selecting all the Orders for this Group... -->

				<Orders>
					<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]/ParentSiblingInfo/parentSiblingInfo/parent/siblings/tradedOrder[generate-id(.)=generate-id(key('clorderid', CLOrderID)[1])]">
            
            <xsl:variable name="ClientOrderID">
              <xsl:choose>
            <xsl:when test="NirvanaMsgType=4">
              <xsl:value-of select="'MANUAL'"/>
              </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ClientOrderID"/>  
            </xsl:otherwise>
            </xsl:choose>
            </xsl:variable>
            
            <Order OrderID="{CLOrderID}" ClOrderID="{$ClientOrderID}" SecondaryOrderID="{ParentClOrderID}"/>

					</xsl:for-each>
				</Orders>

        <Fills>
          <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]/ParentSiblingInfo/parentSiblingInfo/parent/siblings/tradedOrder/fills/fill[generate-id(.)=generate-id(key('FilExecId', ExecutionID)[1])]">

            <Fill ExecID="{ExecutionID}" LastShares="{LastShares}" LastPrice="{LastPrice}"/>
          </xsl:for-each>
        </Fills>
        
			</xsl:for-each>

		</Group>

	</xsl:template>

</xsl:stylesheet>

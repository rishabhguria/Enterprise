<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[IsSwapped='true' and TaxLotState!='Sent']">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
					<!-- <xsl:if test="CounterParty != 'MSCO-S'">  -->
						<!-- ...buid a Group for this node_id -->
						<xsl:call-template name="TaxLotIDBuilder">
							<xsl:with-param name="I_GroupID">
								<xsl:value-of select="PBUniqueID" />
							</xsl:with-param>
						</xsl:call-template>
					 </xsl:if> 
				<!-- </xsl:if> -->
			</xsl:for-each>
		</Groups>
	</xsl:template>

	<xsl:template name="Conversion">
		<xsl:param name="Value"/>
		<xsl:param name="Curr"/>
		<xsl:choose>
			<xsl:when test="Asset='Equity' and IsSwapped='true'">
				<xsl:choose>
					<xsl:when test="FXConversionMethodOperator_Taxlot='M'">
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value * FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="number(FXRate_Taxlot)">
								<xsl:value-of select="$Value div FXRate_Taxlot"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
										<xsl:value-of select="$Value * ForexRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Value div ForexRate"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$Value"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />
		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_GroupID... -->
		<xsl:variable name="QtySum">
			<!--<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>-->
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AllocatedQty)"/>
		</xsl:variable>
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="SoftCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SoftCommissionCharged)"/>
		</xsl:variable>
		<xsl:variable name="TaxOnCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxOnCommissions)"/>
		</xsl:variable>
		<xsl:variable name="StampDutySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/StampDuty)"/>
		</xsl:variable>
		<xsl:variable name="TransactionLevySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TransactionLevy)"/>
		</xsl:variable>
		<xsl:variable name="ClearingFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/ClearingBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="MiscFeesSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/MiscFees)"/>
		</xsl:variable>
		<xsl:variable name="OtherBrokerFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OtherBrokerFee)"/>
		</xsl:variable>
		<xsl:variable name="GrossAmountSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/GrossAmount)"/>
		</xsl:variable>
		<xsl:variable name="NetAmountSum">
			<!--<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>-->
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/NetAmount)"/>
		</xsl:variable>
		<xsl:variable name="SecFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SecFee)"/>
		</xsl:variable>
		<xsl:variable name="OrfFeeSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OrfFee)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>

		<xsl:variable name="tempCPVar">
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
			<!--<xsl:choose>
				<xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
				<xsl:otherwise>NEW</xsl:otherwise>
			</xsl:choose>-->
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
					<xsl:value-of select ="'NEW'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		

		<xsl:variable name="varSymbol">			
			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>
		<xsl:when test="contains(Symbol,'-') or CurrencySymbol !='USD'">
          <xsl:choose>
            <xsl:when test="SEDOL !=''">
              <xsl:value-of select="SEDOL"/>
            </xsl:when>
            <xsl:when test="CUSIP != ''">
              <xsl:value-of select="CUSIP"/>
            </xsl:when>
            <xsl:when test="BBCode != ''">
              <xsl:value-of select="substring-before(BBCode,' ')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="''"/>
            </xsl:otherwise>
          </xsl:choose>				
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="BBCode != ''">
					<xsl:value-of select="substring-before(BBCode,' ')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varSecType">
			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
        <xsl:when test="contains(Symbol,'-') or CurrencySymbol !='USD'">
          <xsl:choose>
            <xsl:when test="contains(UnderlyingSymbol,'BASKET SWAP')">
              <xsl:value-of select="'B'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'S'"/>
            </xsl:otherwise>
          </xsl:choose>					
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="'C'"/>
				</xsl:when>

				<xsl:when test="BBCode != ''">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


    <xsl:variable name="varSettFxAmt">
      <xsl:choose>
        <xsl:when test="SettlCurrency != CurrencySymbol">
          <xsl:choose>
            <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
              <xsl:value-of select="format-number((AveragePrice * FXRate_Taxlot),'0.####')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="format-number((AveragePrice div FXRate_Taxlot),'0.####')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="AveragePrice"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!--<xsl:variable name="GroupGrsAmt">
			<xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
		</xsl:variable>-->
    <xsl:variable name="GroupGrsAmt">
      <xsl:value-of select="$QtySum * AveragePrice * AssetMultiplier"/>
    </xsl:variable>

    <xsl:variable name="varGroupGrsAmt">
      <xsl:choose>
        <xsl:when test="SettlCurrency != CurrencySymbol">
          <xsl:choose>
            <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
              <xsl:value-of select="$GroupGrsAmt * FXRate_Taxlot"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$GroupGrsAmt div FXRate_Taxlot"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$GroupGrsAmt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varGroupGrsAmtt">
      <xsl:choose>
        <xsl:when test="SettlCurrency = CurrencySymbol">
          <xsl:value-of select="$GroupGrsAmt"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varGroupGrsAmt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varPriceAmount">
      <xsl:choose>
        <xsl:when test="SettlCurrency = CurrencySymbol">
          <xsl:value-of select="AveragePrice"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varSettFxAmt"/>
        </xsl:otherwise>
      </xsl:choose>
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

		<xsl:variable name="GroupNetAmt">
			<xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
		</xsl:variable>

		<xsl:variable name="varGroupNetAmnt">
			<xsl:choose>
				<xsl:when test="Asset='Equity' and IsSwapped='true'">
					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Close'">
							<xsl:value-of select="$NetAmountSum - $SecFeeSum"/>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell short'">
							<xsl:value-of select="$NetAmountSum + $SecFeeSum"/>
						</xsl:when>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$NetAmountSum"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


    <xsl:variable name="varSettFxNetAmount">
      <xsl:choose>
        <xsl:when test="SettlCurrency != CurrencySymbol">
          <xsl:choose>
            <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
              <xsl:value-of select="$varGroupNetAmnt * FXRate_Taxlot"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varGroupNetAmnt div FXRate_Taxlot"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varGroupNetAmnt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varNetAmount">
      <xsl:choose>
        <xsl:when test="SettlCurrency = CurrencySymbol">
          <xsl:value-of select="$varGroupNetAmnt"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varSettFxNetAmount"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>


    <xsl:variable name="varNetAmountAbsulate">
      <xsl:choose>
        <xsl:when test="$varNetAmount &gt;0">
          <xsl:value-of select="$varNetAmount"/>
        </xsl:when>
        <xsl:when test="$varNetAmount &lt;0">
          <xsl:value-of select="$varNetAmount * (-1)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

		<xsl:variable name="varEXCODE">
			<xsl:choose>
				<xsl:when test="Symbol = 'TKMR'">
					<xsl:value-of select="'TOR'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="TCounterparty" select="document('../ReconMappingXML/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='MS']/BrokerData[@PranaBroker=$CPVar]/@PBBroker"/>
		<xsl:variable name ="ThirdPartyCounterparty">
			<xsl:choose>
				<xsl:when test="$TCounterparty!=''">
					<xsl:value-of select="$TCounterparty"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'JEFF' or CounterParty= 'ZJEFF'">
					<xsl:value-of select="'JEFF'"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'CITI' or CounterParty= 'ZCITI'">
					<xsl:value-of select="'CITI'"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'JPMS' or CounterParty= 'ZJPMS'">
					<xsl:value-of select="'JPMS'"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'GS' or CounterParty= 'ZGS'">
					<xsl:value-of select="'GS'"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'BERN' or CounterParty= 'ZBERN'">
					<xsl:value-of select="'BERN'"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'CS' or CounterParty= 'ZCS'">
					<xsl:value-of select="'CS'"/>
				</xsl:when>
				<xsl:when test="CounterParty= 'ITGI' or CounterParty= 'ZITGI'">
					<xsl:value-of select="'ITGI'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="CounterParty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPBUniqueIDAndDatePart">
			<xsl:value-of select="concat(PBUniqueID,substring(TradeDate,4,2))"/>
		</xsl:variable>

		<xsl:variable name="PB_NAME" select="'MS'"/>

    <xsl:variable name="varTotalCommission">
      <xsl:value-of select="$CommissionSum + $SoftCommissionSum + $TaxOnCommissionSum"/>
    </xsl:variable>

    <xsl:variable name="varSettCommission">
      <xsl:choose>
        <xsl:when test="SettlCurrency != CurrencySymbol">
          <xsl:choose>
            <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
              <xsl:value-of select="$varTotalCommission * FXRate_Taxlot"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varTotalCommission div FXRate_Taxlot"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varTotalCommission"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varCommission">
      <xsl:choose>
        <xsl:when test ="number($varSettCommission)">
          <xsl:value-of select="$varSettCommission"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="0"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>


    <xsl:variable name="varPriceFXRate">
      <xsl:choose>
	  
	  <xsl:when test="SettlCurrency = 'USD'">
          <xsl:value-of select="'1'"/>
        </xsl:when>
        <!-- <xsl:when test="SettlCurrency != CurrencySymbol"> -->
          <!-- <xsl:value-of select="FXRate_Taxlot"/> -->
        <!-- </xsl:when> -->
        <xsl:otherwise>
           <xsl:value-of select="FXRate_Taxlot"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
	
	
	<xsl:variable name="varSettlCurrency">
      <xsl:choose>
        <xsl:when test="SettlCurrency = CurrencySymbol">
          <xsl:value-of select="CurrencySymbol"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="SettlCurrency"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

		<Group 
				TransactionType = "SW002" TransactionStatus="{$varTaxlotStateGrp}" BuySellIndicator="" LongShortIndicator="" PositionType="{$Sidevar}"
				transactionlevel="B" ClientRefNo="{$varPBUniqueIDAndDatePart}" AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="038211447" AccountID="038211447"
				BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}" TradeDate="{TradeDate}" SettlementDate="{SettlementDate}"
				SettlementCCY="{SettlCurrency}"	ExchangeCode="{$varEXCODE}" Quantity="{$QtySum}" 	PriceAmount="{format-number($varPriceAmount,'###.000000')}"	PriceIndicator="G" Principal="{format-number($varGroupGrsAmtt,'###.00')}" 
				ExecutingBrokerCommission="{format-number($varCommission,'###.0000')}" ExecutingBrokerCommissionIndicator="F" 
				MSFees="" 
				NotRequired=""
				MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varNetAmountAbsulate,'###.00')}" HearsayInd="N" Custodian="MSCO" MoneyManager="" Bookid=""
				Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}" AcquitionDate="" Comments="" SwapReferenceNo="" BasketId=""
				PriceCurrency="{SettlCurrency}" ResetIndicator=""
				SNSreference="" ResearchFee="{format-number($ClearingFeeSum,'#.####')}"   ResearchFeeIndicator="F"  LEI="" 
				EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="">


			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and IsSwapped='true' and TaxLotState!='Sent']">
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

				<xsl:variable name="varTaxlotStateTx">
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
							<xsl:value-of select ="'NEW'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="TaxlotGrsAmt">
					<xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
				</xsl:variable>
				
	<xsl:variable name="varAGroupGrsAmt">
      <xsl:choose>
        <xsl:when test="SettlCurrency != CurrencySymbol">
          <xsl:choose>
            <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
              <xsl:value-of select="$TaxlotGrsAmt * FXRate_Taxlot"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$TaxlotGrsAmt div FXRate_Taxlot"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$TaxlotGrsAmt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varAGroupGrsAmtt">
      <xsl:choose>
        <xsl:when test="SettlCurrency = CurrencySymbol">
          <xsl:value-of select="$TaxlotGrsAmt"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$varAGroupGrsAmt"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

				<xsl:variable name="TaxlotSideMultiplier">
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

				<xsl:variable name="TaxlotNetAmt">
					<xsl:value-of select="$TaxlotGrsAmt + ((CommissionCharged + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions) * $TaxlotSideMultiplier)"/>
				</xsl:variable>

				<xsl:variable name = "PRANA_FUND_NAME">
					<xsl:value-of select="AccountName"/>
				</xsl:variable>

				<xsl:variable name ="THIRDPARTY_FUND_CODE">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
				</xsl:variable>

				<xsl:variable name="FundName_ThirdParty">
					<xsl:choose>
						<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
							<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$PRANA_FUND_NAME"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="ClearingFee1">

					<xsl:call-template name="Conversion">
						<xsl:with-param name="Value" select="ClearingBrokerFee"/>
						<xsl:with-param name="Curr" select="CurrencySymbol"/>
					</xsl:call-template>

				</xsl:variable>

				<xsl:variable name="varAllocatinNetAmnt">
					<xsl:choose>
						<xsl:when test="Asset='Equity' and IsSwapped='true'">
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Close'">
									<xsl:value-of select="NetAmount - SecFee"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell short'">
									<xsl:value-of select="NetAmount + SecFee"/>
								</xsl:when>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="NetAmount"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>



        <xsl:variable name="varASettFxNetAmount">
          <xsl:choose>
            <xsl:when test="SettlCurrency != CurrencySymbol">
              <xsl:choose>
                <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                  <xsl:value-of select="$varAllocatinNetAmnt * FXRate_Taxlot"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varAllocatinNetAmnt div FXRate_Taxlot"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varAllocatinNetAmnt"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="varANetAmount">
          <xsl:choose>
            <xsl:when test="SettlCurrency = CurrencySymbol">
              <xsl:value-of select="$varASettFxNetAmount"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varAllocatinNetAmnt"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>


        <xsl:variable name="varANetAmountAbsulate">
          <xsl:choose>
            <xsl:when test="$varANetAmount &gt;0">
              <xsl:value-of select="$varANetAmount"/>
            </xsl:when>
            <xsl:when test="$varANetAmount &lt;0">
              <xsl:value-of select="$varANetAmount * (-1)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="varATotalCommission">
          <xsl:value-of select="CommissionCharged + SoftCommissionCharged + TaxOnCommissions"/>
        </xsl:variable>

        <xsl:variable name="varASettCommission">
          <xsl:choose>
            <xsl:when test="SettlCurrency != CurrencySymbol">
              <xsl:choose>
                <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                  <xsl:value-of select="$varATotalCommission * FXRate_Taxlot"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$varATotalCommission div FXRate_Taxlot"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varATotalCommission"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name ="varACommission">
          <xsl:choose>
            <xsl:when test ="number($varASettCommission)">
              <xsl:value-of select="$varASettCommission"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select ="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>



        
				<ThirdPartyFlatFileDetail		
					
							Group_Id="" TransactionType = "SW002" TransactionStatus="{$varTaxlotStateTx}" BuySellIndicator="" 
							LongShortIndicator="" PositionType="{$Sidevar}" transactionlevel="A" ClientRefNo="{substring(EntityID,2)}" 
							AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="038211447" AccountID="038CADLA6"
							BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}"
							TradeDate="{TradeDate}" SettlementDate="{SettlementDate}" SettlementCCY="{SettlCurrency}"	ExchangeCode="{$varEXCODE}" 
							Quantity="{AllocatedQty}" PriceAmount="{format-number($varPriceAmount,'###.000000')}" PriceIndicator="G"	
							Principal="{format-number($varAGroupGrsAmtt,'###.00')}" ExecutingBrokerCommission="{format-number($varACommission,'###.0000')}" 
							ExecutingBrokerCommissionIndicator="F" MSFees="" 
							NotRequired=""
							MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varNetAmountAbsulate,'###.00')}" HearsayInd="N" Custodian="MSCO" 
							MoneyManager="" Bookid="" Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" 
							PriceFXRate="{$varPriceFXRate}"
							AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{SettlCurrency}" ResetIndicator="" 
							SNSreference="" ResearchFee="{format-number($ClearingFee1,'#.####')}"   ResearchFeeIndicator="F"  LEI=""
					        EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>


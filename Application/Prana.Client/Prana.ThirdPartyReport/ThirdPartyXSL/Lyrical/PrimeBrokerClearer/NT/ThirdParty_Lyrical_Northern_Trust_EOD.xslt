<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:variable name="GreaterThan" select="'&gt;'"/>
	<xsl:variable name="LessThan" select="'&lt;'"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>			
			
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'true'"/>
					</FileFooter>
					
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<InternalNetNotional>
						<xsl:value-of select="NetAmount"/>
					</InternalNetNotional>

					<ALLOCQTY>
						<xsl:value-of select="AllocatedQty"/>
					</ALLOCQTY>

					<!--for system internal use-->

					<RecordType>
						<xsl:value-of select="'DET'"/>
					</RecordType>

					<GFF>
						<xsl:value-of select="'1.0'"/>
					</GFF>

					<GeneralInfo>
						<xsl:value-of select="concat($LessThan,$LessThan,'GeneralInfo',$GreaterThan,$GreaterThan)"/>
					</GeneralInfo>
					
					<SenderId>
						<xsl:value-of select="'NIRSO1S'"/>
					</SenderId>

					<Destination>
						<xsl:value-of select="'NTL'"/>
					</Destination>

					<SendersReference>
						<xsl:value-of select="substring(EntityID,2)"/>
					</SendersReference>

					<Functionofthemessage>
						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<xsl:value-of select="'N'"/>
							</xsl:when>
							<xsl:when test="TaxLotState='Amended' or TaxLotState='Deleted'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Functionofthemessage>

					<TradeInfo>
						<xsl:value-of select="concat($LessThan,$LessThan,'TradeInfo',$GreaterThan,$GreaterThan)"/>
					</TradeInfo>

					<InstructionType>
						<xsl:choose>
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="'P'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="'D'"/>
							</xsl:when>
						</xsl:choose>
					</InstructionType>

					<!--........................CONFIRM.......................-->
					
					<ProductType>
						<xsl:value-of select="'EQ'"/>
					</ProductType>

					<SettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</SettlementDate>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<xsl:variable name="PB_NAME" select="'Lyrical'"/>

					<xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

					<xsl:variable name="PB_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundName"/>
					</xsl:variable>

					<SafekeepingAccount>
						<xsl:choose>
							<xsl:when test="$PB_FUND_NAME!=''">
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:when>
							<xsl:when test="$PB_FUND_NAME ='Jay H Baker Children&quot;s Trust: 2650389'">
								<xsl:value-of select ="'2650388'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</SafekeepingAccount>

					<TradeType>
						<xsl:value-of select="'TR'"/>
					</TradeType>

					<PlaceofSettlementDepository>
						<xsl:value-of select="'DTCYUS33'"/>
					</PlaceofSettlementDepository>

					<SecurityInfo>
						<xsl:value-of select="concat($LessThan,$LessThan,'SecurityInfo',$GreaterThan,$GreaterThan)"/>
					</SecurityInfo>

					<SecurityIDType>
						<xsl:value-of select="'CU'"/>
					</SecurityIDType>

					<SecurityID>
						<xsl:value-of select="CUSIP"/>
					</SecurityID>

					<SecurityDescription>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityDescription>

					<BrokerInfo>
						<xsl:value-of select="concat($LessThan,$LessThan,'BrokerInfo',$GreaterThan,$GreaterThan)"/>
					</BrokerInfo>

					<ExecutingBrokerBIC>
						<xsl:value-of select="''"/>
					</ExecutingBrokerBIC>

					<xsl:variable name="PRANA_COUNTERPARTY" select="CounterParty"/>

					<xsl:variable name="THRIDPARTY_COUNTERPARTY">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<ExecutingBrokerLocalCode>
						<xsl:choose>
							<xsl:when test="$THRIDPARTY_COUNTERPARTY!=''">
								<xsl:value-of select="$THRIDPARTY_COUNTERPARTY"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBrokerLocalCode>

					<ExecutingBrokerName>
						<xsl:value-of select="''"/>
					</ExecutingBrokerName>

					<ExecutingBrokerAccount>
						<xsl:value-of select="''"/>
					</ExecutingBrokerAccount>

					<ClearingBrokerBIC>
						<xsl:value-of select="''"/>
					</ClearingBrokerBIC>

					<ClearingBrokerLocalCode>
						<xsl:value-of select="''"/>
					</ClearingBrokerLocalCode>

					<ClearingBrokerName>
						<xsl:value-of select="''"/>
					</ClearingBrokerName>

					<ClearingBrokerAccount>
						<xsl:value-of select="''"/>
					</ClearingBrokerAccount>

					<SettlementInfo>
						<xsl:value-of select="concat($LessThan,$LessThan,'SettlementInfo',$GreaterThan,$GreaterThan)"/>
					</SettlementInfo>

					<TradeDate>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</TradeDate>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>

					<TradeAmount>
						<xsl:value-of select="GrossAmount"/>
					</TradeAmount>

					<SettlementCurrency>
						<xsl:value-of select="CurrencySymbol"/>
					</SettlementCurrency>

					<xsl:variable name="Fees">
						<xsl:choose>
							<xsl:when test="(NetAmount - GrossAmount) &gt; 0">
								<xsl:value-of select="NetAmount - GrossAmount"/>
							</xsl:when>
							<xsl:when test="(NetAmount - GrossAmount) &lt; 0">
								<xsl:value-of select="GrossAmount - NetAmount"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SettlementAmount>
						<xsl:value-of select="GrossAmount - $Fees"/>
					</SettlementAmount>

					<DebtFields>
						<xsl:value-of select="concat($LessThan,$LessThan,'DebtFields',$GreaterThan,$GreaterThan)"/>
					</DebtFields>

					<CurrentFace>
						<xsl:value-of select="''"/>
					</CurrentFace>

					<AccruedInterestAmount>
						<xsl:value-of select="AccruedInterest"/>
					</AccruedInterestAmount>

					<LateDeliveryDate>
						<xsl:value-of select="''"/>
					</LateDeliveryDate>

					<MaturityDate>
						<xsl:value-of select="''"/>
					</MaturityDate>

					<InterestRate>
						<xsl:value-of select="''"/>
					</InterestRate>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
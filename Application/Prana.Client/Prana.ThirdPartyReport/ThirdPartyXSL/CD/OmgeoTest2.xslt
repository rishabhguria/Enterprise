<?xml version="1.0" encoding="UTF-8"?>
								<!-- Description: Omgeo Integration, Created Date: 06-15-2012(mm-DD-YY)-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

	<xsl:template name="StringToNumber">
		<xsl:param name="stringValue"/>
		<xsl:choose>
			<xsl:when test="contains($stringValue,'E') or contains($stringValue,'e')">
		<xsl:variable name="vExponent" select="substring-after($stringValue,'E')"/>
		<xsl:variable name="vMantissa" select="substring-before($stringValue,'E')"/>
		<xsl:variable name="vFactor"
             select="substring('100000000000000000000000000000000000000000000',
                               1, substring($vExponent,2) + 1)"/>
		<xsl:choose>
			<xsl:when test="starts-with($vExponent,'-')">
				<xsl:value-of select="$vMantissa div $vFactor"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$vMantissa * $vFactor"/>
			</xsl:otherwise>
		</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$stringValue"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>
			
				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>
				
			<!--2-->
				<MasterReference>
					<xsl:value-of select="'Master Reference'"/>
				</MasterReference>

		    <!--3-->
				<ClientAllocationReference>
					<xsl:value-of select="'ClientAllocationReference'"/>
				</ClientAllocationReference>
				
			<!--4-->
				<L2MatchingProfileName>
					<xsl:value-of select="'L2MatchingProfileName'"/>
				</L2MatchingProfileName>
				
			<!--5-->
				<QuantityOfTheBlockTrade>
					<xsl:value-of select="'QuantityOfTheBlockTrade'"/>
				</QuantityOfTheBlockTrade>
				
			<!--6-->
				<ExecutingBroker>
					<xsl:value-of select="'ExecutingBroker'"/>
				</ExecutingBroker>
				
			<!--7-->
				<OriginatorOfMessage>
					<xsl:value-of select="'OriginatorOfMessage'"/>
				</OriginatorOfMessage>
				
			<!--8 and 9-->
				<TradeDateTime>
					<xsl:value-of select="'TradeDateTime'"/>
				</TradeDateTime>
				
				
			<!--10-->
				<BuySellIndicator>
					<xsl:value-of select="'BuySellIndicator'"/>
				</BuySellIndicator>
			
			<!--11-->
				<DealPrice>
					<xsl:value-of select="'DealPrice'"/>
				</DealPrice>
				
			<!--12-->
				<IdentificationOfASecurity>
					<xsl:value-of select="'IdentificationOfASecurity'"/>
				</IdentificationOfASecurity>
				
			<!--13-->
				<DescriptionOfTheSecurity>
					<xsl:value-of select="'DescriptionOfTheSecurity'"/>
				</DescriptionOfTheSecurity>
				
			<!--14-->
				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>
				</SettlementDate>
				
				
			<!--15-->
				<CurrencyCode>
					<xsl:value-of select="'CurrencyCode'"/>
				</CurrencyCode>
				
			<!--16-->
				<TradeTransactionConditionIndicator>
					<xsl:value-of select="'TradeTransactionConditionIndicator'"/>
				</TradeTransactionConditionIndicator>
				
			<!--17-->
				<TotalTradeAmount>
					<xsl:value-of select="'TotalTradeAmount'"/>
				</TotalTradeAmount>	
				
			<!--18-->
				<PartyCapacityIndicator>
					<xsl:value-of select="'PartyCapacityIndicator'"/>
				</PartyCapacityIndicator>
				
			<!--19-->
				<TradeCommAmt>
					<xsl:value-of select="'TradeCommAmt'"/>
				</TradeCommAmt>
				
			<!--20-->
				<PlaceOfSafekeepingValue>
					<xsl:value-of select="'PlaceOfSafekeepingValue'"/>
				</PlaceOfSafekeepingValue>
				
			<!--21-->
				<PlaceOfSafekeepingPlace>
					<xsl:value-of select="'PlaceOfSafekeepingPlace'"/>
				</PlaceOfSafekeepingPlace>
				
			<!--22-->
				<TypeOfPriceIndicator>
					<xsl:value-of select="'TypeOfPriceIndicator'"/>
				</TypeOfPriceIndicator>
				
			<!--13-->
				<SettlementInstructionsSourceIndicator>
					<xsl:value-of select="'SettlementInstructionsSourceIndicator'"/>
				</SettlementInstructionsSourceIndicator>
				
			<!--14-->
				<AccountID>
					<xsl:value-of select="'AccountID'"/>
				</AccountID>
				
			<!--15-->
				<AlertCountryCode>
					<xsl:value-of select="'AlertCountryCode'"/>
				</AlertCountryCode>
				
			<!--16-->
				<AlertSecurityType>
					<xsl:value-of select="'AlertSecurityType'"/>
				</AlertSecurityType>
				
			<!--17-->
				<QuantityAllocated>
					<xsl:value-of select="'QuantityAllocated'"/>
				</QuantityAllocated>
				
			<!--18-->
				<NetCashAmount>
					<xsl:value-of select="'NetCashAmount'"/>
				</NetCashAmount>
				
			<!--19-->
				<TradeAmount>
					<xsl:value-of select="'TradeAmount'"/>
				</TradeAmount>
				
			<!--20-->
				<ThirdPartyToTrade>
					<xsl:value-of select="'ThirdPartyToTrade'"/>
				</ThirdPartyToTrade>
				
			<!--21-->
				<PartyType>
					<xsl:value-of select="'PartyType'"/>
				</PartyType>
				
				
				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>				

			</ThirdPartyFlatFileDetail>
			
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="true"/>
					</RowHeader>
					
					<!--for system use only-->					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>

					<!--for system internal use-->
					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>
					

					<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
					<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

					<!--2-->
					<MasterReference>
						<xsl:value-of select="TradeRefID"/>
					</MasterReference>
					
					<!--3-->
					<ClientAllocationReference>
						<xsl:value-of select="EntityID"/>
					</ClientAllocationReference>
					
					<!--4-->
					<L2MatchingProfileName>
						<xsl:value-of select="''"/>
					</L2MatchingProfileName>
					
					<!--5-->
					<QuantityOfTheBlockTrade>
						<xsl:value-of select="OrderQty"/>
					</QuantityOfTheBlockTrade>
					
					<!--6-->
					<ExecutingBroker>
						<xsl:value-of select="CounterParty"/>
					</ExecutingBroker>
					
					<!--7-->
					<OriginatorOfMessage>
						<xsl:value-of select="''"/>
					</OriginatorOfMessage>
					
					<!--8 and 9-->
					<TradeDateTime>
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'),' 000000')"/> 
					<!--	<xsl:value-of select="TradeDateTime"/>  -->	
					</TradeDateTime>
					
					<!--10-->
					<xsl:variable name="varPosType">
						<xsl:choose>
							<xsl:when test="Side='Buy to Open' or Side='Buy'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test=" Side='Sell' or Side='Sell to Close'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							<xsl:when test="Side='Buy to Close'">
								<xsl:value-of select="'B'"/>
							</xsl:when>							
						</xsl:choose>
					</xsl:variable>
					
					<BuySellIndicator>
						<xsl:value-of select="$varPosType"/>
					</BuySellIndicator>
					
					<!--11-->
					<DealPrice>
						<xsl:value-of select="AveragePrice"/>
					</DealPrice>
					
					<!--12-->
					<IdentificationOfASecurity>
						<xsl:choose>
							<xsl:when test ="Asset='Equity'">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</IdentificationOfASecurity>
					
					<!--13-->
					<DescriptionOfTheSecurity>
						<xsl:value-of select="FullSecurityName"/>
					</DescriptionOfTheSecurity>
					
					<!--14-->
					<SettlementDate>
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'),' 000000')"/>
						<!--<xsl:value-of select="SettlementDate"/>-->
					</SettlementDate>
					
					<!--15-->
					<CurrencyCode>
					<xsl:value-of select ="CurrencySymbol"/>
					</CurrencyCode>
					
					<!--16-->
					<TradeTransactionConditionIndicator>
					<xsl:value-of select ="''"/>
					</TradeTransactionConditionIndicator>
					
					<!--17-->
					<TotalTradeAmount>
						<xsl:value-of select='OrderQty * AveragePrice'/>
					</TotalTradeAmount>
					
					<!--18-->
					<PartyCapacityIndicator>
					<xsl:value-of select ="''"/>
					</PartyCapacityIndicator>
					
					<!--19-->
					<TradeCommAmt>
					<xsl:value-of select ="''"/>
					</TradeCommAmt>
					
					<!--20-->
					<PlaceOfSafekeepingValue>
					<xsl:value-of select ="''"/>
					</PlaceOfSafekeepingValue>
					
					<!--21-->
					<PlaceOfSafekeepingPlace>
					<xsl:value-of select ="''"/>
					</PlaceOfSafekeepingPlace>
					
					<!--22-->
					<TypeOfPriceIndicator>
					<xsl:value-of select ="''"/>
					</TypeOfPriceIndicator>
					
					<!--13-->
					<SettlementInstructionsSourceIndicator>
						<xsl:value-of select="'O'"/>
					</SettlementInstructionsSourceIndicator>
					
					<!--14-->
					<AccountID>
					<xsl:value-of select ="AccountNo"/>
					</AccountID>
					
					<!--15-->
					<AlertCountryCode>
						<xsl:value-of select="''"/>
					</AlertCountryCode>
					
					<!--16-->
					<AlertSecurityType>
						<xsl:value-of select="''"/>
					</AlertSecurityType>
					
					<!--17-->
					<QuantityAllocated>
						<xsl:value-of select="AllocatedQty"/>
					</QuantityAllocated>
					
					<!--18-->
					<NetCashAmount>
						<xsl:choose>
							<xsl:when test="TradeCcy != 'USD'">
								<xsl:value-of select='format-number((NetAmount * ForexRate_Trade), "###.00")'/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select='format-number((NetAmount * 1), "###.00")'/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select='NetAmount'/>-->
					</NetCashAmount>
					
					<!--19-->
					<TradeAmount>
						<xsl:value-of select='AllocatedQty * AveragePrice'/>
					</TradeAmount>
					
					<!--20-->
					<xsl:variable name="varFundName">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name="varPB">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_BrokerMatch.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaFundName = $varFundName]/@PB"/>
					</xsl:variable>
					
					<xsl:variable name="varBIC">
						<xsl:value-of select="document('../ReconMappingXml/BIC_Omgeo.xml')/BICMapping/PB[@Name='SENSATO']/BrokerData[@PBName = $varPB]/@BIC"/>
					</xsl:variable>

					<ThirdPartyToTrade>
						<xsl:choose>
							<xsl:when test="$varPB != ''">
								<xsl:value-of select ="$varPB"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ThirdPartyToTrade>
					
					<!--21-->
					<PartyType>
						<xsl:choose>
							<xsl:when test="$varBIC != ''">
								<xsl:value-of select ="$varBIC"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</PartyType>
					
					
					</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

					
					
					

					
					
					
					
					
					
					
					

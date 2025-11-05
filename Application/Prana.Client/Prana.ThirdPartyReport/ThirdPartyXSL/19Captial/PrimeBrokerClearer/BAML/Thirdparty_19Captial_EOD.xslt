<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

			
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

			
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<trddate>
					<xsl:value-of select ="'Trade Date'"/>
				</trddate>



				<Side>
                     <xsl:value-of select="'Side'"/>

				</Side>



				<Symbol>
					
					<xsl:value-of select="'Symbol'"/>
				</Symbol>

				<Qty>
					<xsl:value-of select="'Qty'"/>
				</Qty>

				<Px>
					<xsl:value-of select="'Px'"/>
				</Px>

				<OpenCloseIndicator>
						<xsl:value-of select="'OpenClose Indicator'"/>		
				</OpenCloseIndicator>

				<Underlier>	
					<xsl:value-of select="'Underlier'"/>	
				</Underlier>

				<Strike>
					
					<xsl:value-of select="'Strike'"/>
				</Strike>

				<CP>	
					<xsl:value-of select="'C/P'"/>	
				</CP>

				<Expiry>
					<xsl:value-of select="'Expiry'"/>
				</Expiry>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>


			
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>
			
			
			<xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption']">
				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'Ture'"/>
					</RowHeader>
					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<xsl:variable name="PB_NAME" select="'19Captial'"/>
					
					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBrokerCode"/>
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

					<Broker>
						<xsl:value-of select="$Broker"/>
					</Broker>

					<TradeDate>
						<xsl:value-of select ="TradeDate"/>
					</TradeDate>

				

					<Side>

						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="Side='Buy to Open' or Side='Buy'">
										<xsl:value-of select="'B'"/>
									</xsl:when>

									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						</xsl:choose>
						
					</Side>
					
					

					<Symbol>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:value-of select="concat(substring-before(normalize-space(OSIOptionSymbol),' '),substring-after(normalize-space(OSIOptionSymbol),' '))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<Qty>

						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="number(AllocatedQty)">
										<xsl:value-of select ="AllocatedQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						</xsl:choose>			
						
					</Qty>

					<Px>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="number(AveragePrice)">
										<xsl:value-of select ="AveragePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						</xsl:choose>
								
					</Px>

					<OpenCloseIndicator>
						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<xsl:choose>
									<xsl:when test="Side='Buy' or Side='Buy to Open'">
										<xsl:value-of select="'O'"/>
									</xsl:when>
									<xsl:when test="Side='Sell' or Side='Sell to Close'">
										<xsl:value-of select="'C'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
									
								</xsl:choose>
							</xsl:when>
						</xsl:choose>
					</OpenCloseIndicator>

					<Underlier>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="UnderlyingSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Underlier>

					<Strike>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Strike>

					<CP>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="substring(PutOrCall,1,1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CP>

					<Expiry>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="ExpirationDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Expiry>

					

					<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<Commission>
						<xsl:value-of select="$COMM"/>
					</Commission>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>

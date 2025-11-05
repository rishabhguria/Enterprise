<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="COL1 != 'Symbol'">
					<PositionMaster>

						
								<AccountName>
									<xsl:value-of select='COL3'/>
								</AccountName>


						<Commission>
							<xsl:value-of select ='90'/>
						</Commission>
						
						<SoftCommission>
							<xsl:value-of select ='10'/>
						</SoftCommission>

						<SecFee>
							<xsl:value-of select ='20'/>
						</SecFee>

						<OccFee>
							<xsl:value-of select ='30'/>
						</OccFee>

						<OrfFee>
							<xsl:value-of select ='40'/>
						</OrfFee>

						<StampDuty>
							<xsl:value-of select ='60'/>
							</StampDuty>

						<TaxOnCommissions>
							<xsl:value-of select ='70'/>
						</TaxOnCommissions>

						<TransactionLevy>
							<xsl:value-of select ='50'/>
						</TransactionLevy>

						<ClearingFee>
							<xsl:value-of select ='70'/>
						</ClearingFee>

						<MiscFees>
							<xsl:value-of select ='80'/>

						</MiscFees>

						<Fees>
							<xsl:value-of select ='60'/>
						</Fees>

						<ClearingBrokerFee>
							<xsl:value-of select ='50'/>
						</ClearingBrokerFee>
												
						<!--QUANTITY-->

						<NetPosition>
							<xsl:value-of select="COL2"/>
						</NetPosition>
						
						<xsl:choose>
							<xsl:when test="boolean(number(COL3))">
								<CostBasis>
									<xsl:value-of select="COL3"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<!--Side-->
						<SideTagValue>
							<xsl:value-of select="COL4"/>
						</SideTagValue>
						
						<!--<xsl:choose>
							<xsl:when test="COL5='sell to open'">
								<SideTagValue>
									<xsl:value-of select="'C'"/>
									</SideTagValue>
							</xsl:when>
							<xsl:when test="COL5='Buy to open'">
								<SideTagValue>
									<xsl:value-of select="'A'"/>
								</SideTagValue>
							</xsl:when>


							<xsl:when test="COL5='BUY'">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL5='sell short'">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
							</xsl:when>
							<xsl:when test="COL5='SELL'">
								<SideTagValue>
									<xsl:value-of select="'2'"/>
								</SideTagValue>
							</xsl:when>

							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
							</xsl:otherwise>
						</xsl:choose>-->

						
								<Symbol>
									<xsl:value-of select='COL1'/>
								</Symbol>
							

						

						<!--COMMISSION-->
						
					<PositionStartDate>
						<xsl:value-of select="''"/>
					</PositionStartDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
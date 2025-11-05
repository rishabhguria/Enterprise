<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name ="varSide">
					<xsl:value-of select ="translate(COL3,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="$varSide = 'Buy' or $varSide = 'Sell short' or $varSide = 'Sell' or $varSide = 'Buy to Close' or $varSide = 'Sell to Close' or $varSide = 'Sell to Open' or $varSide = 'Buy to Open'" >
					<PositionMaster>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varSide='Purchase'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy to Open'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell to Close'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell to Open'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
						
						<CounterPartyID>
						<xsl:choose>
						<xsl:when test="$varSide='cpid'">
									<xsl:value-of select="23"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="24"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<Symbol>
							<xsl:value-of select ="translate(COL2,'&quot;','')"/>
						</Symbol>
						
						<FXRate>
							<xsl:value-of select ="translate(COL9,'&quot;','')"/>
						</FXRate>
							<TransactionType>
							<xsl:value-of select ="translate(COL23,'&quot;','')"/>
						</TransactionType>
						
						<AccountName>
							<xsl:value-of select ="translate(COL4,'&quot;','')"/>
						</AccountName>
						
						<xsl:variable name ="varPrice">
							<xsl:value-of select ="translate(COL7,'&quot;','')"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test="number($varPrice)">
									<xsl:value-of select="number($varPrice)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
						
						<xsl:variable name ="varQuantity">
							<xsl:value-of select ="translate(COL5,'&quot;','')"/>
						</xsl:variable>

						<NetPosition>
						
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="number($varQuantity)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						
						<xsl:variable name ="varComm">
							<xsl:value-of select ="translate(COL11,'&quot;','')"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="number($varComm)">
									<xsl:value-of select="number($varComm)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>
						
						<xsl:variable name ="varSComm">
							<xsl:value-of select ="translate(COL12,'&quot;','')"/>
						</xsl:variable>

						<SoftCommission>
							<xsl:choose>
								<xsl:when test="number($varSComm)">
									<xsl:value-of select="number($varSComm)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SoftCommission>
						
						<xsl:variable name ="varStamp">
							<xsl:value-of select ="translate(COL15,'&quot;','')"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="number($varStamp)">
									<xsl:value-of select="number($varStamp)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>
						
						<xsl:variable name ="varClearingBrokerFee">
							<xsl:value-of select ="translate(COL14,'&quot;','')"/>
						</xsl:variable>

						<ClearingBrokerFee>
							<xsl:choose>
								<xsl:when test="number($varClearingBrokerFee)">
									<xsl:value-of select="number($varClearingBrokerFee)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingBrokerFee>
						
						<xsl:variable name ="varTransactionLevy">
							<xsl:value-of select ="translate(COL16,'&quot;','')"/>
						</xsl:variable>

						<TransactionLevy>
							<xsl:choose>
								<xsl:when test="number($varTransactionLevy)">
									<xsl:value-of select="number($varTransactionLevy)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>
						
						<xsl:variable name ="varSecFee">
							<xsl:value-of select ="translate(COL20,'&quot;','')"/>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="number($varSecFee)">
									<xsl:value-of select="number($varSecFee)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						
						<xsl:variable name ="varOccFee">
							<xsl:value-of select ="translate(COL21,'&quot;','')"/>
						</xsl:variable>

						<OccFee>
							<xsl:choose>
								<xsl:when test="number($varOccFee)">
									<xsl:value-of select="number($varOccFee)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OccFee>
						
						<xsl:variable name ="varClearingFee">
							<xsl:value-of select ="translate(COL17,'&quot;','')"/>
						</xsl:variable>

						<ClearingFee>
							<xsl:choose>
								<xsl:when test="number($varClearingFee)">
									<xsl:value-of select="number($varClearingFee)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</ClearingFee>
						
						<xsl:variable name ="varMiscFees">
							<xsl:value-of select ="translate(COL19,'&quot;','')"/>
						</xsl:variable>

						<MiscFees>
							<xsl:choose>
								<xsl:when test="number($varMiscFees)">
									<xsl:value-of select="number($varMiscFees)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MiscFees>
						
						<xsl:variable name ="varOrfFee">
							<xsl:value-of select ="translate(COL22,'&quot;','')"/>
						</xsl:variable>

						<OrfFee>
							<xsl:choose>
								<xsl:when test="number($varOrfFee)">
									<xsl:value-of select="number($varOrfFee)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>
						
						<xsl:variable name ="varTaxOnCommissions">
							<xsl:value-of select ="translate(COL18,'&quot;','')"/>
						</xsl:variable>

						<TaxOnCommissions>
							<xsl:choose>
								<xsl:when test="number($varTaxOnCommissions)">
									<xsl:value-of select="number($varTaxOnCommissions)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TaxOnCommissions>
						
						<PositionStartDate>
							<xsl:value-of select ="translate(COL1,'&quot;','')"/>
						</PositionStartDate>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
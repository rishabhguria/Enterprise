<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template match="/">
	<DocumentElement>
		
		<xsl:for-each select="//PositionMaster">
			
			<xsl:if test="number(COL8)">
				
				<PositionMaster>

					<xsl:variable name = "var_PB_NAME" >
						<xsl:value-of select="'ML'"/>
					</xsl:variable>
					
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL5"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$var_PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountName>

					<Description>
						<xsl:value-of select="COL4"/>
					</Description>

					<!--<Strategy>
						<xsl:value-of select="COL20"/>				
					</Strategy>-->

					<xsl:variable name="PB_SYMBOL_NAME">
						<xsl:value-of select="normalize-space(COL4)"/>
					</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$var_PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
					</xsl:variable>

					<Symbol>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</xsl:when>

							<xsl:when test="COL3!='*'">
								<xsl:value-of select="''"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="COL3"/>
							</xsl:otherwise>

						</xsl:choose>

					</Symbol>
					<Bloomberg>

						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME!=''">
								<xsl:value-of select="''"/>
							</xsl:when>

							<xsl:when test="COL3!='*'">
								<xsl:value-of select="normalize-space(COL3)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>

					</Bloomberg>
					

					<PositionStartDate>
						<xsl:value-of select="COL1"/>
					</PositionStartDate>

					<SideTagValue>
						<xsl:choose>
							<xsl:when test="COL7 = 'Buy'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL7 = 'Sell'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
								<xsl:when test="COL7 = 'Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL7 = 'Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
									<xsl:when test="COL7 = 'Buy to Open'">
										<xsl:value-of select="'9'"/>
									</xsl:when>
									<xsl:when test="COL7 = 'Buy to Close'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
							<xsl:when test="COL7 = 'Sell to Open'">
								<xsl:value-of select="'11'"/>
							</xsl:when>
							<xsl:when test="COL7 = 'Sell to Closee'">
								<xsl:value-of select="'12'"/>
							</xsl:when>
									<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SideTagValue>					

					<NetPosition>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL8)) &gt; 0">
								<xsl:value-of select="COL8"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL8)) &lt; 0">
								<xsl:value-of select="COL8* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>

					<!--<CostBasis>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL10)) &gt; 0">
								<xsl:value-of select="COL10"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL10)) &lt; 0">
								<xsl:value-of select="COL10* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CostBasis>-->

					<xsl:variable name="CostBasis">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL11 div COL8"/>
						</xsl:call-template>
					</xsl:variable>

					<CostBasis>
						<xsl:choose>
							<xsl:when test="$CostBasis &gt; 0">
								<xsl:value-of select="$CostBasis"/>

							</xsl:when>
							<xsl:when test="$CostBasis &lt; 0">
								<xsl:value-of select="$CostBasis * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</CostBasis>
					

					<PositionSettlementDate>
						<xsl:value-of select="COL1"/>
					</PositionSettlementDate>

					<!--<Description>
						<xsl:value-of select="COL18"/>
					</Description>-->


					<!--<Commission>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL14)) &gt; 0">
								<xsl:value-of select="COL14"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL14)) &lt; 0">
								<xsl:value-of select="COL14* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>-->

					<!--<LotId>
						<xsl:value-of select="COL10"/>
					</LotId>-->

					<!--<ExternalTransId>
						<xsl:value-of select="COL11"/>
					</ExternalTransId>-->

					<!--<xsl:variable name="varFees">
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL15)) ">
								<xsl:value-of select="COL15"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>-->

					<!--<xsl:variable name="varOtherFee">
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL16)) ">
								<xsl:value-of select="COL16"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>-->
					<!--<Fees>
						<xsl:value-of select="$varFees+$varOtherFee"/>
					</Fees>-->

					<OriginalPurchaseDate>
						<xsl:value-of select="COL2"/>
					</OriginalPurchaseDate>

					<xsl:variable name="TransactionType">
						<xsl:value-of select="COL21"/>
					</xsl:variable>

					<TransactionType>
							<xsl:choose>
								<xsl:when test="$TransactionType='Long Addition'">
									<xsl:value-of select="'LongAddition'"/>
								</xsl:when>
								<xsl:when test="$TransactionType='Long Withdrawal'">
									<xsl:value-of select="'LongWithdrawal'"/>
								</xsl:when>
								<xsl:when test="$TransactionType='Short Addition'">
									<xsl:value-of select="'ShortAddition'"/>
								</xsl:when>
								<xsl:when test="$TransactionType='Short Withdrawal'">
									<xsl:value-of select="'ShortWithdrawal'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionType>
					<FXConversionMethodOperator>
						<xsl:value-of select="COL20"/>
					</FXConversionMethodOperator>
					<PBSymbol>
						<xsl:value-of select="$PB_SYMBOL_NAME"/>
					</PBSymbol>

					<FXRate>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL19)) &gt; 0">
								<xsl:value-of select="COL19"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL19)) &lt; 0">
								<xsl:value-of select="COL19* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose> 
					</FXRate>
					
				</PositionMaster>
				
			</xsl:if>
			
		</xsl:for-each>
		
	</DocumentElement>
	
 </xsl:template>

	<xsl:variable name="varLower" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varUpper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet> 

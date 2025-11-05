<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="NetPosition">
					<xsl:value-of select ="COL10"/>
				</xsl:variable>

				<xsl:if test ="number($NetPosition) and not(contains(COL4,'RTS/'))">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name = "PB_CURRENCY_NAME" >
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME and $PB_CURRENCY_NAME = @Currency]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL6,'.')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<!--<xsl:variable name ="vaxExFaxtor">
							<xsl:value-of select="substring(string-length(substring-before(COL6,'.')),1)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL6!=''">
									<xsl:choose>
										<xsl:when test="$vaxExFaxtor='a'">
											<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_a',$PRANA_SUFFIX_NAME)"/>
										</xsl:when>
										<xsl:when test="$PRANA_SUFFIX_NAME!=''">
											<xsl:value-of select="concat(substring-before(COL6,'.'),$PRANA_SUFFIX_NAME)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL6"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>-->

						<xsl:variable name="varExFactor">
							<xsl:value-of select="substring(COL6,string-length(substring-before(COL6,'.')),1)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL6!=''">
									<xsl:choose>

										<xsl:when test="$varExFactor='a'">
											<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_A',$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:when test="$varExFactor='b'">
											<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_B',$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:when test="$PRANA_SUFFIX_NAME!=''">
											<xsl:value-of select="concat(substring-before(COL6,'.'),$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="COL6"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select ="''"/>
						</PositionStartDate>

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<xsl:variable name="varSide" select="COL3"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$varSide ='Long'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>

								<xsl:when test ="$varSide ='Short'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select="number(COL14)"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<CounterPartyID>
							<xsl:value-of select="'17'"/>
						</CounterPartyID>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="substring(COL5,1,3)='SDB'">
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="translate(concat($PRANA_SYMBOL_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
										</xsl:when>

										<xsl:when test="COL6!=''">
											<xsl:choose>

												<xsl:when test="$varExFactor='a'">
													<xsl:value-of select="translate(concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_A',$PRANA_SUFFIX_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
												</xsl:when>

												<xsl:when test="$varExFactor='b'">
													<xsl:value-of select="translate(concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_B',$PRANA_SUFFIX_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
												</xsl:when>

												<xsl:when test="$PRANA_SUFFIX_NAME!=''">
													<xsl:value-of select="translate(concat(substring-before(COL6,'.'),$PRANA_SUFFIX_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="translate(concat(COL6,'_swap'),$upper_CONST,$lower_CONST)"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="translate(concat($PB_SYMBOL_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:when test="COL6!=''">
											<xsl:choose>

												<xsl:when test="$varExFactor='a'">
													<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_A',$PRANA_SUFFIX_NAME)"/>
												</xsl:when>

												<xsl:when test="$varExFactor='b'">
													<xsl:value-of select="concat(substring(COL6,1,string-length(substring-before(COL6,'.'))-1),'_B',$PRANA_SUFFIX_NAME)"/>
												</xsl:when>

												<xsl:when test="$PRANA_SUFFIX_NAME!=''">
													<xsl:value-of select="concat(substring-before(COL6,'.'),$PRANA_SUFFIX_NAME)"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="COL6"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</TradeAttribute1>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>

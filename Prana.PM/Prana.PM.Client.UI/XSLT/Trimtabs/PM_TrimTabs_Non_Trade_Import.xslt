<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'$',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	
	
	<xsl:template name="FormatDate">
		<xsl:param name="DateTime" />
		<!-- converts date time double number to 18/12/2009 -->

		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019" />
		</xsl:variable>

		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))" />
		</xsl:variable>

		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
		</xsl:variable>

		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
		</xsl:variable>

		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
		</xsl:variable>

		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))" />
		</xsl:variable>

		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
		</xsl:variable>

		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))" />
		</xsl:variable>

		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
		</xsl:variable>

		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
		</xsl:variable>

		<xsl:variable name ="varMonthUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nMonth) = 1">
					<xsl:value-of select ="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="nDayUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nDay) = 1">
					<xsl:value-of select ="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL8)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varDescription" select="normalize-space(COL2)"/>

				<xsl:if test="number($Cash) and ($varDescription ='INVESTMENT INTEREST RECEIVABLE' or $varDescription ='ACCRUED UNITARY FEE EXPENSE' or 
						$varDescription ='NET UNREAL CRNCY APPR ON INVESTMENTS' or $varDescription ='NET UNREAL CRNCY DEPR ON INVESTMENTS' 
					or $varDescription ='MARK TO MARKET')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="PRANA_ACRONYM_NAME_PRE">
							<xsl:choose>
								<xsl:when test="$varDescription ='INVESTMENT INTEREST RECEIVABLE' and $Cash &gt; 0">
									<xsl:value-of select="'Interest_Receivable'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='INVESTMENT INTEREST RECEIVABLE' and $Cash &lt; 0">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='ACCRUED UNITARY FEE EXPENSE' and $Cash &gt; 0">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='ACCRUED UNITARY FEE EXPENSE' and $Cash &lt; 0">
									<xsl:value-of select="'ACCRUEDUNITARYFEE'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY APPR ON INVESTMENTS' and $Cash &gt; 0">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY APPR ON INVESTMENTS' and $Cash &lt; 0">
									<xsl:value-of select="'MTM'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY DEPR ON INVESTMENTS' and $Cash &gt; 0">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY DEPR ON INVESTMENTS' and $Cash &lt; 0">
									<xsl:value-of select="'MTM'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='MARK TO MARKET' and $Cash &gt; 0">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='MARK TO MARKET' and $Cash &lt; 0">
									<xsl:value-of select="'MTM'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_ACRONYM_NAME_POST">
							<xsl:choose>
								<xsl:when test="$varDescription ='INVESTMENT INTEREST RECEIVABLE' and $Cash &gt; 0">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='INVESTMENT INTEREST RECEIVABLE' and $Cash &lt; 0">
									<xsl:value-of select="'Interest_Receivable'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='ACCRUED UNITARY FEE EXPENSE' and $Cash &gt; 0">
									<xsl:value-of select="'ACCRUEDUNITARYFEE'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='ACCRUED UNITARY FEE EXPENSE' and $Cash &lt; 0">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY APPR ON INVESTMENTS' and $Cash &gt; 0">
									<xsl:value-of select="'MTM'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY APPR ON INVESTMENTS' and $Cash &lt; 0">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY DEPR ON INVESTMENTS' and $Cash &gt; 0">
									<xsl:value-of select="'MTM'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='NET UNREAL CRNCY DEPR ON INVESTMENTS' and $Cash &lt; 0">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='MARK TO MARKET' and $Cash &gt; 0">
									<xsl:value-of select="'MTM'"/>
								</xsl:when>
								<xsl:when test="$varDescription ='MARK TO MARKET' and $Cash &lt; 0">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="$Cash * -1"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<JournalEntries>
							<xsl:value-of select="concat($PRANA_ACRONYM_NAME_PRE, ':' , $AbsCash , '|' , $PRANA_ACRONYM_NAME_POST, ':' , $AbsCash)"/>
						</JournalEntries>

						<CurrencyName>
							<xsl:value-of select ="normalize-space(COL12)"/>
						</CurrencyName>

						<Date>
							<xsl:value-of select ="''"/>
						</Date>

						<Description>
							<xsl:value-of select="$varDescription"/>
						</Description>
					
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>